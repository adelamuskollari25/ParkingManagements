using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.Ticket
{
    public class TicketCloseDTO
    {
        public Guid TicketId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsLostTicket { get; set; }
    }

}
