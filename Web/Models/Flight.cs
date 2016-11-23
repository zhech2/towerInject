using System;

namespace Web.Models
{
    public class Flight
    {
        public DateTimeOffset DepartureDateTime { get; set; }

        public DateTimeOffset ArrivalDateTime { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }
    }
}