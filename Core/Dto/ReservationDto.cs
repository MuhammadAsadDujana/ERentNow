using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public string ReservedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Nullable<bool> IsViewedFlag { get; set; }
    }
}
