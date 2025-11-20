using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment : BaseEntity
{
    public Guid TicketId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime PaidAt { get; set; }
    public virtual Ticket Ticket { get; set; }
}