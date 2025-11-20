using System.ComponentModel.DataAnnotations;

namespace ParkingManagements.Data.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 
    }
}
