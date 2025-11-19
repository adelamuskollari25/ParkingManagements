using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.DTOs.Ticket;
using ParkingManagements.Server.Interfaces;
using ParkingManagements.Server.Services.Helpers;

namespace ParkingManagements.Server.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TicketDTO> CreateAsync(TicketCreateDTO dto)
        {
            var spot = await _context.ParkingSpots.FindAsync(dto.SpotId);

            if (spot == null)
                throw new Exception("Spot not found.");

            if (spot.Status != SpotStatus.Free)
                throw new Exception("Spot is not available.");

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Plate == dto.PlateNumber);

            if (vehicle == null)
            {
                vehicle = new Vehicle { Plate = dto.PlateNumber };
                _context.Vehicles.Add(vehicle);
            }

            var ticket = new Ticket
            {
                LotId = dto.LotId,
                SpotId = dto.SpotId,
                Vehicle = vehicle,
                EntryTime = DateTime.UtcNow,
                Status = TicketStatus.Open
            };

            spot.Status = SpotStatus.Occupied;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDTO>(ticket);
        }

        public async Task<TicketDTO> PreviewExitAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Vehicle)
                .Include(t => t.ParkingLot)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found.");

            var exitTime = DateTime.UtcNow;

            var tariff = await _context.Tariffs
                .Where(t => t.LotId == ticket.LotId)
                .OrderByDescending(t => t.EffectiveFrom)
                .FirstAsync();

            var amount = TariffCalculator.Calculate(tariff, ticket.EntryTime, exitTime);
            ticket.ComputedAmount = amount;

            return _mapper.Map<TicketDTO>(ticket);
        }


        public async Task<TicketDTO> CloseAndPayAsync(TicketCloseDTO dto)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(t => t.Id == dto.TicketId);

            if (ticket == null)
                throw new Exception("Ticket not found.");

            if (ticket.Status == TicketStatus.Closed)
                throw new Exception("Ticket already closed.");

            ticket.ExitTime = DateTime.UtcNow;

            var tariff = await _context.Tariffs
                .FirstAsync(t => t.LotId == ticket.LotId);

            decimal amount;

            if (dto.IsLostTicket && tariff.LostTicketFee.HasValue)
            {
                amount = tariff.LostTicketFee.Value;
            }
            else
            {
                amount = TariffCalculator.Calculate(tariff, ticket.EntryTime, ticket.ExitTime.Value);
            }

            ticket.ComputedAmount = amount;
            ticket.Status = TicketStatus.Closed;
            ticket.Paid = true;

            var payment = new Payment
            {
                TicketId = ticket.Id,
                Amount = amount,
                Method = dto.PaymentMethod,
                PaidAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            var spot = await _context.ParkingSpots.FindAsync(ticket.SpotId);
            if (spot != null)
                spot.Status = SpotStatus.Free;

            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDTO>(ticket);
        }


        public async Task<IEnumerable<TicketDTO>> SearchAsync(TicketSearchDTO filters)
        {
            var query = _context.Tickets
                .Include(t => t.Vehicle)
                .AsQueryable();

            if (filters.Status.HasValue)
                query = query.Where(t => t.Status == filters.Status);

            if (!string.IsNullOrWhiteSpace(filters.Plate))
                query = query.Where(t => t.Vehicle.Plate.Contains(filters.Plate));

            if (filters.LotId.HasValue)
                query = query.Where(t => t.LotId == filters.LotId);

            if (filters.SpotId.HasValue)
                query = query.Where(t => t.SpotId == filters.SpotId);

            if (filters.From.HasValue)
                query = query.Where(t => t.EntryTime >= filters.From);

            if (filters.To.HasValue)
                query = query.Where(t => t.EntryTime <= filters.To);

            var tickets = await query.ToListAsync();

            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
    }
}
