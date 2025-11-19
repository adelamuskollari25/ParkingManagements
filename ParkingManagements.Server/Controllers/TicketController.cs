using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Server.DTOs.Ticket;
using ParkingManagements.Server.Interfaces;

namespace ParkingManagements.Server.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("enter")]
        public async Task<IActionResult> Enter(TicketCreateDTO dto)
        {
            var result = await _ticketService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("{ticketId}/preview-exit")]
        public async Task<IActionResult> PreviewExit(Guid ticketId)
        {
            var result = await _ticketService.PreviewExitAsync(ticketId);
            return Ok(result);
        }

        [HttpPost("close")]
        public async Task<IActionResult> Close(TicketCloseDTO dto)
        {
            var result = await _ticketService.CloseAndPayAsync(dto);
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(TicketSearchDTO filters)
        {
            var result = await _ticketService.SearchAsync(filters);
            return Ok(result);
        }
    }

}
