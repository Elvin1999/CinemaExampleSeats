using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Model
{
    public class Movie
    {
    
        public string MovieName { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public override string ToString()
        {
            return $"{MovieName} {Date} {Time}  {Location}";
        }

    }
}
