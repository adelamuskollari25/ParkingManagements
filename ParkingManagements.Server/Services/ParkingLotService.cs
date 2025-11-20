using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.ParkingLot;
using ParkingManagements.Server.Interfaces;

public class ParkingLotService : IParkingLotService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ParkingLotService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<PagedResult<ParkingLotDTO>> GetAllLotsAsync(PaginationParams pagination)
    {
        if (pagination.PageNumber <= 0)
            pagination.PageNumber = 1;

        var query = _context.ParkingLots.AsQueryable();
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        var items = await query
            .OrderBy(l => l.Name)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ProjectTo<ParkingLotDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<ParkingLotDTO>
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


    public async Task<ParkingLotDTO?> GetLotByIdAsync(Guid lotId)
    {
        var lot = await _context.ParkingLots
            .ProjectTo<ParkingLotDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(l => l.Id == lotId);

        if (lot == null)
            throw new ServiceException("notfound", $"Parking lot with ID {lotId} not found.", 404);

        return lot;
    }

    public async Task<ParkingLotDTO> CreateLotAsync(ParkingLotCreateDTO dto)
    {
        var lot = _mapper.Map<ParkingLot>(dto);
        lot.Id = Guid.NewGuid();

        _context.ParkingLots.Add(lot);
        await _context.SaveChangesAsync();

        return _mapper.Map<ParkingLotDTO>(lot);
    }


    public async Task<ParkingLotKpiDTO?> GetLotOccupancyMetricsAsync(Guid lotId)
    {
        var lot = await _context.ParkingLots
            .Include(l => l.ParkingSpots)
            .Include(l => l.Tickets)
            .ThenInclude(t => t.Payments)
            .FirstOrDefaultAsync(l => l.Id == lotId);

        if (lot == null)
            throw new ServiceException("notfound", $"Parking lot with ID {lotId} not found.", 404);

        var revenueToday = lot.Tickets
            .SelectMany(t => t.Payments)
            .Where(p => p.PaidAt.Date == DateTime.UtcNow.Date)
            .Sum(p => p.Amount);

        return new ParkingLotKpiDTO
        {
            Id = lot.Id,
            TotalSpots = lot.ParkingSpots.Count,
            FreeSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Free),
            OccupiedSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Occupied),
            UnavailableSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Unavailable)
        };
    }
}
