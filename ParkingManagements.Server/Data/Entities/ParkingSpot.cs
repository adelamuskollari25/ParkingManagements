using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ParkingSpots")]
public class ParkingSpot : BaseEntity
{
    public Guid LotId { get; set; }

    public string SpotCode { get; set; }
    public SpotType Type { get; set; }
    public SpotStatus Status { get; set; }

    public virtual ParkingLot ParkingLot { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}