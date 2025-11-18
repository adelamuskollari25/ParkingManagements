using Microsoft.AspNetCore.Identity;
using ParkingManagements.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkingManagements.server.Data.Entities
{
    public class User : IdentityUser
    {
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public bool Active { get; set; } = true;
        public string PasswordHash { get; set; }
    }
}
