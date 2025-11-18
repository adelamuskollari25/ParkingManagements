using ParkingManagement.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

public class ParkingLot : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Timezone { get; set; }

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}