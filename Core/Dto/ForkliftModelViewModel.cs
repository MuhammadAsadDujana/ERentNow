using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Dto
{
    public class ForkliftModelViewModel
    {
        public decimal TransportationPickUp { get; set; }
        public decimal TransportationDropOffPickUp { get; set; }
        public decimal TransportationDropOffOnly { get; set; }
        public decimal TransportationPickUpOnly { get; set; }
        public decimal InsuranceLossDamageWaiver { get; set; }
        public decimal InsurancePurchasePrice { get; set; }
        public string Make { get; set; }
        public decimal Capacity { get; set; }
        public string ForkliftModel { get; set; }
        public int Quantity { get; set; }
        public int ForkliftId { get; set; }
        public decimal MaxMeter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricingStandardDaily { get; set; }
        public decimal PricingStandardgWeekly { get; set; }
        public decimal PricingStandardMonthly { get; set; }
        public decimal NationalAccountDaily { get; set; }
        public decimal NationalAccountWeekly { get; set; }
        public decimal NationalAccountMonthly { get; set; }
        public decimal Environmental { get; set; }
        public decimal MiscCharges { get; set; }
        public HttpPostedFileBase imageUpload { get; set; }
        public string lblEnvironmental { get; set; }
        public string lblMiscCharges { get; set; }
        public virtual ICollection<ForkliftsModelCharge> ForkliftsModelCharges { get; set; }
    }
}
