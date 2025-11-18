using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
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

    public async Task<IEnumerable<ParkingLotDTO>> GetAllLotsAsync()
    {
        return await _context.ParkingLots
            .ProjectTo<ParkingLotDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ParkingLotDTO?> GetLotByIdAsync(Guid lotId)
    {
        return await _context.ParkingLots
            .ProjectTo<ParkingLotDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(l => l.Id == lotId);
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
            .FirstOrDefaultAsync(l => l.Id == lotId);

        if (lot == null) return null;

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
