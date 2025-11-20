using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.Interfaces;

namespace ParkingManagements.Server.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(Guid ticketId, [FromQuery] PaginationParams pagination)
        {
            var result = await _paymentService.GetPaymentsForTicketAsync(ticketId, pagination);
            return Ok(result);
        }
    }

}
