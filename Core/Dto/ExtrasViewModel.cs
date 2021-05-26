using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ExtrasViewModel
    {
        public string Environmental { get; set; }
        public string MiscCharges { get; set; }
        public decimal EnvironmentalAmount { get; set; }
        public decimal MiscChargesAmount { get; set; }
    }
}
