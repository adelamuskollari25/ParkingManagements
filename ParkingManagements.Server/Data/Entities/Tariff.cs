using ParkingManagements.Data.Entities;

public class Tariff : BaseEntity
{
    public Guid LotId { get; set; }
    public decimal RatePerHour { get; set; }
    public int BillingPeriodMinutes { get; set; }
    public int GracePeriodMinutes { get; set; }
    public decimal? DailyMaximum { get; set; }
    public decimal? LostTicketFee { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public virtual ParkingLot ParkingLot { get; set; }
}