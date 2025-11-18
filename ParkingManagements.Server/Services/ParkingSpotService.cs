using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
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

    public async Task<IEnumerable<ParkingSpotDTO>> GetSpotsByLotAsync(Guid lotId)
    {
        return await _context.ParkingSpots
            .Where(s => s.LotId == lotId)
            .ProjectTo<ParkingSpotDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ParkingSpotDTO?> GetSpotByIdAsync(Guid spotId)
    {
        return await _context.ParkingSpots
            .ProjectTo<ParkingSpotDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(s => s.Id == spotId);
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
        if (spot == null) return false;

        spot.SpotCode = dto.SpotCode;
        spot.Type = dto.Type;
        spot.Status = dto.Status; 

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeSpotStatusAsync(Guid spotId, SpotStatus newStatus)
    {
        var spot = await _context.ParkingSpots.FindAsync(spotId);
        if (spot == null) return false;

        spot.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }
}
