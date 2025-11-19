using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.ParkingSpot
{
    public class ParkingSpotUpdateDTO
    {
        public string SpotCode { get; set; }
        public SpotType Type { get; set; }
        public SpotStatus Status { get; set; }
    }
}
