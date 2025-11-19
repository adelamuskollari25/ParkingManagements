namespace ParkingManagements.Server.Services.Helpers
{
    public static class TariffCalculator
    {
        public static decimal Calculate(Tariff tariff, DateTime entry, DateTime exit)
        {
            if (exit <= entry)
                return 0;

            var totalMinutes = (exit - entry).TotalMinutes;

            if (totalMinutes <= tariff.GracePeriodMinutes)
                return 0;

            totalMinutes -= tariff.GracePeriodMinutes;

            var billingPeriods = Math.Ceiling(totalMinutes / tariff.BillingPeriodMinutes);

            var amount = (decimal)billingPeriods * tariff.RatePerHour;

            if (tariff.DailyMaximum.HasValue)
            {
                var days = (exit.Date - entry.Date).Days + 1;
                var maxTotal = days * tariff.DailyMaximum.Value;

                if (amount > maxTotal)
                    amount = maxTotal;
            }

            return amount;
        }
    }

}
