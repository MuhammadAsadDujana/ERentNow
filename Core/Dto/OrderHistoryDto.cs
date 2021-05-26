using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class OrderHistoryDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public string Picture { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Location { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
