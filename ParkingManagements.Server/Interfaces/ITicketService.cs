using ParkingManagements.Server.DTOs.Ticket;

namespace ParkingManagements.Server.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDTO> CreateAsync(TicketCreateDTO dto);
        Task<TicketDTO> PreviewExitAsync(Guid ticketId);
        Task<TicketDTO> CloseAndPayAsync(TicketCloseDTO dto);

        Task<IEnumerable<TicketDTO>> SearchAsync(TicketSearchDTO filters);
    }

}
