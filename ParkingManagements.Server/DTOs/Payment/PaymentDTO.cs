using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.Payment
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
