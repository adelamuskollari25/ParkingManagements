using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.Ticket
{
    public class TicketCreateDTO
    {
        public Guid LotId { get; set; }
        public Guid SpotId { get; set; }
        public string PlateNumber { get; set; }
        public VehicleType? VehicleType { get; set; }  // NEW
        public string? Color { get; set; } // NEW
    }

}
