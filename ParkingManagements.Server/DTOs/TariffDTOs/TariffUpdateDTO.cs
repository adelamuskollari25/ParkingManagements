namespace ParkingManagements.Server.DTOs.Tariff
{
    public class TariffUpdateDTO
    {
        public decimal RatePerHour { get; set; }
        public int BillingPeriodMinutes { get; set; }
        public int GracePeriodMinutes { get; set; }
        public decimal? DailyMaximum { get; set; }
        public decimal? LostTicketFee { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
