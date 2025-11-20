namespace ParkingManagements.Server.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;
    }
}
