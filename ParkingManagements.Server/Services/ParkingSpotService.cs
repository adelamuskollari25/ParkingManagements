using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.Common.Sortings;
using ParkingManagements.Server.DTOs.ParkingSpot;
using ParkingManagements.Server.Interfaces;

public class ParkingSpotService : IParkingSpotService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ParkingSpotService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<ParkingSpotDTO>> GetSpotsByLotAsync(Guid lotId, ParkingSpotFilterParams filters)
    {
        var query = _context.ParkingSpots
            .Where(s => s.LotId == lotId)
            .AsQueryable();

        if (filters.Status.HasValue)
            query = query.Where(s => s.Status == filters.Status.Value);

        if (filters.Type.HasValue)
            query = query.Where(s => s.Type == filters.Type.Value);

        if (!string.IsNullOrWhiteSpace(filters.SpotCode))
            query = query.Where(s => s.SpotCode.Contains(filters.SpotCode));

        query = filters.SortBy?.ToLower() switch
        {
            "spotcode" => filters.SortDescending ? query.OrderByDescending(s => s.SpotCode) : query.OrderBy(s => s.SpotCode),
            "type" => filters.SortDescending ? query.OrderByDescending(s => s.Type) : query.OrderBy(s => s.Type),
            "status" => filters.SortDescending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
            _ => query.OrderBy(s => s.SpotCode)
        };

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filters.PageSize);

        var items = await query
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ProjectTo<ParkingSpotDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<ParkingSpotDTO>
        {
            Data = items,
            Meta = new PaginationMeta
            {
                CurrentPage = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPrevious = filters.PageNumber > 1,
                HasNext = filters.PageNumber < totalPages
            }
        };
    }

    public async Task<ParkingSpotDTO?> GetSpotByIdAsync(Guid spotId)
    {
        var spot = await _context.ParkingSpots
            .ProjectTo<ParkingSpotDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(s => s.Id == spotId);

        if (spot == null)
            throw new ServiceException("spot_not_found", "Parking spot not found.", 404);

        return spot;
    }

    public async Task<ParkingSpotDTO> CreateSpotAsync(Guid lotId, ParkingSpotCreateDTO dto)
    {
        var spot = _mapper.Map<ParkingSpot>(dto);
        spot.Id = Guid.NewGuid();
        spot.LotId = lotId;

        _context.ParkingSpots.Add(spot);
        await _context.SaveChangesAsync();

        return _mapper.Map<ParkingSpotDTO>(spot);
    }

    public async Task<bool> UpdateSpotAsync(Guid spotId, ParkingSpotUpdateDTO dto)
    {
        var spot = await _context.ParkingSpots.FindAsync(spotId);
        if (spot == null)
            throw new ServiceException("spot_not_found", "Parking spot not found.", 404);

        spot.SpotCode = dto.SpotCode;
        spot.Type = dto.Type;
        spot.Status = dto.Status;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeSpotStatusAsync(Guid spotId, SpotStatus newStatus)
    {
        var spot = await _context.ParkingSpots.FindAsync(spotId);
        if (spot == null)
            throw new ServiceException("spot_not_found", "Parking spot not found.", 404);

        spot.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }
}
