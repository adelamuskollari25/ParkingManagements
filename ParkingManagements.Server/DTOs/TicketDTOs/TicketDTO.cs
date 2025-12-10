using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.Ticket
{
    public class TicketDTO
    {
        public Guid Id { get; set; }
        public Guid SpotId { get; set; }
        public Guid LotId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public TicketStatus Status { get; set; }
        public string SpotCode { get; set; } // NEW
        public decimal? ComputedAmount { get; set; }
        public bool Paid { get; set; }
        public VehicleDTO Vehicle { get; set; }
}

    public class VehicleDTO
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public VehicleType? Type { get; set; }  // NEW
        public string? Color { get; set; }      // NEW
    }

}
