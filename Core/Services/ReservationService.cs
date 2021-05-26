using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
   public class ReservationService
    {
        eRentEntities DbContext;

        public ReservationService()
        {
            DbContext = new eRentEntities();
        }

        public ReservationHdr Add(ReservationVM reservationVM)
        {
            ReservationHdr model = new ReservationHdr();

            return model;
        }
    }
}
