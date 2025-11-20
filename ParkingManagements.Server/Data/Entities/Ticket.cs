using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket : BaseEntity
{
    public Guid LotId { get; set; }
    public Guid SpotId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public TicketStatus Status { get; set; }
    public decimal? ComputedAmount { get; set; }
    public bool Paid { get; set; } = false;
    public virtual ParkingLot ParkingLot { get; set; }
    public virtual ParkingSpot ParkingSpot { get; set; }
    public virtual Vehicle Vehicle { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}