using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ReservationHeaderDto
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string DocumentNumber { get; set; }
        public string CustomerPO { get; set; }
        public decimal TotalRentAmount { get; set; }
        public string ReservationStatus { get; set; }
        public int ReservationHdrsId { get; set; }
        public string Insurance { get; set; }
    }
}
