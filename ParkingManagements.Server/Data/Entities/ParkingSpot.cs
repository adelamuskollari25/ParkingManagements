using ParkingManagement.Data.Entities;
using ParkingManagement.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

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