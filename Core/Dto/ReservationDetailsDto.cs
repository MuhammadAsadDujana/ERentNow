using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ReservationDetailsDto
    {
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Location { get; set; }
        public decimal RentPrice { get; set; }
        public string ReservationStatus { get; set; }
        public int ReservationId { get; set; }
    }
}
