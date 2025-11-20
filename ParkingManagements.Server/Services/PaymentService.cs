using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Payment;
using ParkingManagements.Server.Interfaces;

namespace ParkingManagements.Server.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<PaymentDTO>> GetPaymentsForTicketAsync(Guid ticketId, PaginationParams pagination)
        {
            var query = _context.Payments
                .Where(p => p.TicketId == ticketId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .OrderBy(p => p.PaidAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var mappedItems = _mapper.Map<IEnumerable<PaymentDTO>>(items);

            return new PagedResult<PaymentDTO>
            {
                Data = mappedItems,
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
    }
}
