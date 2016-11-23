using System;
using System.Collections.Concurrent;
using System.Text;
using Web.Models;

namespace Web.Services
{
    public class BookingService : IBookingService
    {
        private readonly IEmailService _emailService;
        private ConcurrentDictionary<string, Booking> _bookingMap = new ConcurrentDictionary<string, Booking>();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private Random _rand = new Random();

        public BookingService(IEmailService emailService)
        {
            if (emailService == null)
            {
                throw new ArgumentNullException(nameof(emailService));
            }

            _emailService = emailService;
        }

        public Booking GetByRecordLocator(string recordLocator)
        {
            Booking booking;
            _bookingMap.TryGetValue(recordLocator, out booking);

            return booking;
        }


        public Booking Commit(Booking booking)
        {
            _emailService.SendEmail();

            if (String.IsNullOrEmpty(booking.RecordLocator))
            {
                booking.RecordLocator = createRecordLocator();
            }
            _bookingMap.AddOrUpdate(booking.RecordLocator, booking, (b, oldBooking) => booking);

            return booking;
        }

        private string createRecordLocator()
        {
            var recordLocator = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                var pos = _rand.Next() % Chars.Length;
                recordLocator.Append(Chars[pos]);
            }

            return recordLocator.ToString();
        }
    }
}