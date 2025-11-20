using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.Common.Sortings
{
    public class ParkingSpotFilterParams : PaginationParams
    {
        public SpotStatus? Status { get; set; }
        public SpotType? Type { get; set; }
        public string? SpotCode { get; set; }
        public string? SortBy { get; set; } 
        public bool SortDescending { get; set; } = false;
    }
}
