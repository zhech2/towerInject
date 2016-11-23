using System.Collections.Generic;

namespace Web.Models
{
    public class Booking
    {
        private ICollection<Passenger> _passengers;
        private ICollection<Flight> _flights;

        public string RecordLocator { get; set; }

        public ICollection<Passenger> Passengers
        {
            get { return _passengers ?? (_passengers = new List<Passenger>()); }
            set { _passengers = value; }
        }

        public ICollection<Flight> Flights
        {
            get { return _flights ?? (_flights = new List<Flight>()); }
            set { _flights = value; }
        }
    }
}