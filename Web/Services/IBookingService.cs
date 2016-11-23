using Web.Models;

namespace Web.Services
{
    public interface IBookingService
    {
        Booking GetByRecordLocator(string recordLocator);
        Booking Commit(Booking booking);
    }
}