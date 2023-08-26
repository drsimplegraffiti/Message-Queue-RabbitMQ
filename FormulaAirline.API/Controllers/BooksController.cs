
using FormulaAirline.API.Data;
using FormulaAirline.API.Models;
using FormulaAirline.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirline.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BooksController> _logger;
        private readonly IMessageProducer _messageProducer;

        public BooksController(
            ILogger<BooksController> logger,
            IMessageProducer messageProducer,
            AppDbContext context)
        {
            _logger = logger;
            _messageProducer = messageProducer;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            // _messageProducer.SendingMessage(booking);
            _messageProducer.SendingMessage<Booking>(booking); // This is the same as the line above
            return Ok(booking);
        }
    }
}