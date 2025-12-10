using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Tariff;
using ParkingManagements.Server.Interfaces;

public class TariffService : ITariffService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TariffService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TariffDTO?> GetCurrentTariffAsync(Guid lotId)
    {
        return await _context.Tariffs
            .Where(t => t.LotId == lotId)
            .OrderByDescending(t => t.EffectiveFrom)
            .ProjectTo<TariffDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<TariffDTO>> GetTariffHistoryAsync(Guid lotId, PaginationParams pagination)
    {
        var query = _context.Tariffs
            .Where(t => t.LotId == lotId)
            .OrderByDescending(t => t.EffectiveFrom)
            .AsQueryable();

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectTo<TariffDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TariffDTO>
        {
            Data = items,
            Meta = new PaginationMeta
            {
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPrevious = pagination.PageNumber > 1,
                HasNext = pagination.PageNumber < totalPages
            }
        };
    }

    public async Task<TariffDTO> UpdateTariffAsync(Guid lotId, TariffUpdateDTO dto)
    {
        var tariff = new Tariff
        {
            Id = Guid.NewGuid(),
            LotId = lotId,
            RatePerHour = dto.RatePerHour,
            BillingPeriodMinutes = dto.BillingPeriodMinutes,
            GracePeriodMinutes = dto.GracePeriodMinutes,
            DailyMaximum = dto.DailyMaximum,
            LostTicketFee = dto.LostTicketFee,
            EffectiveFrom = dto.EffectiveFrom
        };

        _context.Tariffs.Add(tariff);
        await _context.SaveChangesAsync();

        return _mapper.Map<TariffDTO>(tariff);
    }
}
