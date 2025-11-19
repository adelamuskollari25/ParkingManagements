using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.DTOs.Metrics___Reportings;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ReportingService : IReportingService
{
    private readonly ApplicationDbContext _context;

    public ReportingService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<LotSnapshotDTO>> GetPerLotSnapshotAsync()
    {
        var today = DateTime.UtcNow.Date;

        var lots = await _context.ParkingLots
            .Include(l => l.ParkingSpots)
            .ToListAsync();

        var snapshots = new List<LotSnapshotDTO>();

        foreach (var lot in lots)
        {
            var openTicketsToday = await _context.Tickets
                .Where(t => t.LotId == lot.Id && t.Status == TicketStatus.Open)
                .CountAsync();

            var revenueToday = await _context.Payments
                .Include(p => p.Ticket)
                .Where(p => p.Ticket.LotId == lot.Id && p.PaidAt.Date == today)
                .SumAsync(p => p.Amount);

            snapshots.Add(new LotSnapshotDTO
            {
                LotId = lot.Id,
                Name = lot.Name,
                TotalSpots = lot.ParkingSpots.Count,
                OccupiedSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Occupied),
                FreeSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Free),
                UnavailableSpots = lot.ParkingSpots.Count(s => s.Status == SpotStatus.Unavailable),
                OpenTickets = openTicketsToday,
                RevenueToday = revenueToday
            });
        }

        return snapshots;
    }


    public async Task<IEnumerable<DailyRevenueDTO>> GetDailyRevenueSummaryAsync(DateTime from, DateTime to)
    {
        var payments = await _context.Payments
            .Include(p => p.Ticket)
            .Where(p => p.PaidAt.Date >= from.Date && p.PaidAt.Date <= to.Date)
            .ToListAsync();

        var grouped = payments
            .GroupBy(p => p.PaidAt.Date)
            .Select(g => new DailyRevenueDTO
            {
                Date = g.Key,
                Revenue = g.Sum(p => p.Amount)
            })
            .OrderBy(d => d.Date)
            .ToList();

        return grouped;
    }
}
