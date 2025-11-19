namespace ParkingManagements.Server.DTOs.ParkingLot
{
    public class ParkingLotKpiDTO
    {
        public Guid Id { get; set; }
        public int TotalSpots { get; set; }
        public int FreeSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int UnavailableSpots { get; set; }
    }
}
