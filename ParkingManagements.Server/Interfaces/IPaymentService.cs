using ParkingManagements.Server.DTOs.Payment;

namespace ParkingManagements.Server.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetPaymentsForTicketAsync(Guid ticketId);

    }
}
