using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ForkliftsModelChargesDto
    {
        public int ForkliftsModelId { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; }
    }
}
