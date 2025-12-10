using ParkingManagements.Data.Entities.Enums;

namespace ParkingManagements.Server.Common.Sortings
{
    public class TicketFilterParams : PaginationParams
    {
        public TicketStatus? Status { get; set; }
        public string? Plate { get; set; }
        public Guid? LotId { get; set; }
        public Guid? SpotId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
