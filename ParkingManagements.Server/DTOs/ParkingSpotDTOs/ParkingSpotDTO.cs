using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.DTOs.ParkingSpot
{
    public class ParkingSpotDTO
    {
        public Guid Id { get; set; }
        public string SpotCode { get; set; }
        public SpotType Type { get; set; }
        public SpotStatus Status { get; set; }
    }
}
