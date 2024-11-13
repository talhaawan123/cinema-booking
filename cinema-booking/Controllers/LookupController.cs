using cinema_booking.DataContext.DatabaseContext;
using cinema_booking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private const decimal TicketPrice = 100m;
        public LookupController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("cinemas")]
        public async Task<List<Cinema>> GetCinemas()
        {
            return await _context.Cinemas.ToListAsync();
        }
        [HttpGet("cinema/{cinemaId}/movies")]
        public async Task<ActionResult<List<Movie>>> GetMoviesByCinema(int cinemaId)
        {
            var cinema = await _context.Cinemas.Include(c => c.Movies)
                                                .FirstOrDefaultAsync(c => c.Id == cinemaId);

            if (cinema == null)
            {
                return NotFound("Cinema not found.");
            }

            return Ok(cinema.Movies);
        }

        [HttpGet("movies")]
        public async Task<List<Movie>> GetMovies()
        {
            return await _context.Movies.Include(c => c.SeatList).ToListAsync();
        }

        [HttpPost("{movieId}/reserve")]
        public async Task<ActionResult> ReserveSeats(int movieId, [FromBody] List<string> seatNumbers)
        {
            var movie = await _context.Movies.Include(m => m.SeatList)
                                             .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            var availableSeats = movie.SeatList
                                      .Where(s => seatNumbers.Contains(s.SeatNumber) && !s.IsReserved)
                                      .ToList();

            if (availableSeats.Count != seatNumbers.Count)
            {
                return BadRequest("One or more selected seats are not available.");
            }
          
            decimal totalPrice = availableSeats.Count * TicketPrice;  // Calculate total price

            // Mark the selected seats as reserved
            foreach (var seat in availableSeats)
            {
                seat.IsReserved = true;
            }

            await _context.SaveChangesAsync();

            // Return the total price with a success message
            return Ok(new { Message = "Seats successfully reserved.", TotalPrice = totalPrice });
        }
    }
}
