using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Data.Entities
{
    public class Vehicle : BaseEntity
    {
        public string Plate { get; set; }

        public VehicleType? Type { get; set; }
        public string? Color { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
