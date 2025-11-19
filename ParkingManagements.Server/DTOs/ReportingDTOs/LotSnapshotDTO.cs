namespace ParkingManagements.Server.DTOs.Metrics___Reportings
{
    public class LotSnapshotDTO
    {
        public Guid LotId { get; set; }
        public string Name { get; set; }
        public int TotalSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int FreeSpots { get; set; }
        public int UnavailableSpots { get; set; }
        public int OpenTickets { get; set; }
        public decimal RevenueToday { get; set; }
    }
}
