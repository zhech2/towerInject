using System;
using System.Net;
using System.Web.Http;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Route("api/booking")]
    public class BookingController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IBookingService _bookingService;

        public BookingController(ILogger logger, IBookingService bookingService)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (bookingService == null)
            {
                throw new ArgumentNullException(nameof(bookingService));
            }

            _logger = logger;
            _bookingService = bookingService;
        }

        public IHttpActionResult Get(string recordLocator)
        {
            if (recordLocator == null)
            {
                return BadRequest();
            }

            var booking = _bookingService.GetByRecordLocator(recordLocator);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        public IHttpActionResult Post(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var committedBooking = _bookingService.Commit(booking);
            if (committedBooking == null)
            {
                return StatusCode((HttpStatusCode)422);
            }

            _logger.Debug("Created booking");
            return Created($"/api/Booking?recordLocator={committedBooking.RecordLocator}", new { });
        }
    }
}
