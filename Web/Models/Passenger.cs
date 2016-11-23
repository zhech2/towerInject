namespace Web.Models
{
    public class Passenger
    {
        private Name _name;

        public Name Name
        {
            get { return _name ?? (_name = new Name()); }
            set { _name = value; }
        }
    }
}