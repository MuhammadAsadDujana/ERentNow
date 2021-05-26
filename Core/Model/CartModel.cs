using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
   public class CartModel : ForkliftsModel
    {
        public string UserID { get; set; }
        public int? ModelID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public dynamic ForkliftsCharges { get; set; }
    }
}
