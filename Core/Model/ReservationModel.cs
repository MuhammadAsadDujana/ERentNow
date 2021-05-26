using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Model
{
    public class ReservationModel
    {
        public ReservationModel()
        {
            this.ReservationList = new List<Reservation>();
        }

        public List<Reservation> ReservationList { get; set; }
        public string ReservationLine { get; set; }
    }

    public class ReservationVM : ReservationHdr
    {
        public HttpPostedFileBase Contract { get; set; }
        public HttpPostedFileBase InsuranceFile { get; set; }
        public string Status { get; set; }

        public IEnumerable<ReservationDtlVM> ListReservationDtl { get; set; }
    }


    public class ReservationDtlVM : ReservationDtl
    {
        public string Model { get; set; }
        public string Make { get; set; }
    }
}
