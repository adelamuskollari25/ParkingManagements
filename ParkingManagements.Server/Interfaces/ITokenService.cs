using ParkingManagements.server.Data.Entities;

namespace ParkingManagements.Server.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, IList<string> roles);
    }
}
