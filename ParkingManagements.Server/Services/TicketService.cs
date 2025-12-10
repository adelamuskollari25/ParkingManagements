using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.Common.Sortings;
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
                throw new ServiceException("spot_not_found", "Spot not found.", 404);

            if (spot.Status != SpotStatus.Free)
                throw new ServiceException("spot_not_available", "Spot is not available.", 409);

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
                throw new ServiceException("ticket_not_found", "Ticket not found.", 404);

            var exitTime = DateTime.UtcNow;

            var tariff = await _context.Tariffs
                .Where(t => t.LotId == ticket.LotId)
                .OrderByDescending(t => t.EffectiveFrom)
                .FirstAsync();

            ticket.ComputedAmount = TariffCalculator.Calculate(tariff, ticket.EntryTime, exitTime);

            return _mapper.Map<TicketDTO>(ticket);
        }

        public async Task<TicketDTO> CloseAndPayAsync(TicketCloseDTO dto)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(t => t.Id == dto.TicketId);

            if (ticket == null)
                throw new ServiceException("ticket_not_found", "Ticket not found.", 404);

            if (ticket.Status == TicketStatus.Closed)
                throw new ServiceException("ticket_already_closed", "Ticket already closed.", 409);

            ticket.ExitTime = DateTime.UtcNow;

            var tariff = await _context.Tariffs
                .FirstAsync(t => t.LotId == ticket.LotId);

            decimal amount = dto.IsLostTicket && tariff.LostTicketFee.HasValue
                ? tariff.LostTicketFee.Value
                : TariffCalculator.Calculate(tariff, ticket.EntryTime, ticket.ExitTime.Value);

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

        public async Task<PagedResult<TicketDTO>> SearchAsync(TicketFilterParams filters)
        {
            var query = _context.Tickets
                .Include(t => t.Vehicle)
                .Include(t => t.ParkingSpot) // NEW: REQUIRED for SpotCode
                .AsQueryable();

            if (filters.Status.HasValue)
                query = query.Where(t => t.Status == filters.Status.Value);

            if (!string.IsNullOrWhiteSpace(filters.Plate))
                query = query.Where(t => t.Vehicle.Plate.Contains(filters.Plate));

            if (filters.LotId.HasValue)
                query = query.Where(t => t.LotId == filters.LotId);

            if (filters.SpotId.HasValue)
                query = query.Where(t => t.SpotId == filters.SpotId);

            if (filters.From.HasValue)
                query = query.Where(t => t.EntryTime >= filters.From.Value);

            if (filters.To.HasValue)
                query = query.Where(t => t.EntryTime <= filters.To.Value);

            query = filters.SortBy?.ToLower() switch
            {
                "entrytime" => filters.SortDescending ? query.OrderByDescending(t => t.EntryTime) : query.OrderBy(t => t.EntryTime),
                "exittime" => filters.SortDescending ? query.OrderByDescending(t => t.ExitTime) : query.OrderBy(t => t.ExitTime),
                "status" => filters.SortDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
                "amount" => filters.SortDescending ? query.OrderByDescending(t => t.ComputedAmount) : query.OrderBy(t => t.ComputedAmount),
                "plate" => filters.SortDescending ? query.OrderByDescending(t => t.Vehicle.Plate) : query.OrderBy(t => t.Vehicle.Plate),
                _ => query.OrderBy(t => t.EntryTime)
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filters.PageSize);

            var items = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ProjectTo<TicketDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TicketDTO>
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
    }
}
