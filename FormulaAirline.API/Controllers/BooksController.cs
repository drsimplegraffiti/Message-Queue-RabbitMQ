
using FormulaAirline.API.Data;
using FormulaAirline.API.Models;
using FormulaAirline.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.PassengerName == booking.PassengerName);
            
            if(existingBooking != null)
                return BadRequest(new ErrorResponse(){
                    Message = "Booking already exists",
                    StatusCode = 400
                });

            _logger.LogInformation("Creating booking");

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            // _messageProducer.SendingMessage(booking);
            _messageProducer.SendingMessage<Booking>(booking); // This is the same as the line above
            return Ok(booking);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if(booking == null)
                return NotFound();

            return Ok(booking);
        }

        // create error response class
        public class ErrorResponse
        {
            public string Message { get; set; } = "An error occured";
            public string StackTrace { get; set; } = "An error occured";
            public int StatusCode { get; set; } = 400;
        }
}
}