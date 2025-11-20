using ParkingManagements.Server.Common;
using ParkingManagements.Server.Common.Sortings;
using ParkingManagements.Server.DTOs.Ticket;

namespace ParkingManagements.Server.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDTO> CreateAsync(TicketCreateDTO dto);
        Task<TicketDTO> PreviewExitAsync(Guid ticketId);
        Task<TicketDTO> CloseAndPayAsync(TicketCloseDTO dto);
        Task<PagedResult<TicketDTO>> SearchAsync(TicketFilterParams filters);
    }

}
