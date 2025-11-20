namespace ParkingManagements.Server.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
