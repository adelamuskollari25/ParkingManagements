using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Server.DTOs.Payment;
using ParkingManagements.Server.Interfaces;
using System;

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

        public async Task<IEnumerable<PaymentDTO>> GetPaymentsForTicketAsync(Guid ticketId)
        {
            var payments = await _context.Payments
                .Where(p => p.TicketId == ticketId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }
    }

}
