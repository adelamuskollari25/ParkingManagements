using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Payment;

namespace ParkingManagements.Server.Interfaces
{
    public interface IPaymentService
    {
        Task<PagedResult<PaymentDTO>> GetPaymentsForTicketAsync(Guid ticketId, PaginationParams pagination);

    }
}
