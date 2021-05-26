using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ReservationForkliftsModelDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int ReservationQty { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string Location { get; set; }
        public string InsurancePicture { get; set; }
        public string Status { get; set; }
        public string ReservedBy { get; set; }
        public System.DateTime ReservedAt { get; set; }
        public Nullable<bool> IsViewedFlag { get; set; }

        public int Id { get; set; }
        public int ForkliftsId { get; set; }
        public string Picture { get; set; }
        public string Make { get; set; }
        public decimal Capacity { get; set; }
        public string Model { get; set; }
        public decimal MaxMeter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal PricingStandard_Daily { get; set; }
        public decimal PricingStandard_Weekly { get; set; }
        public decimal PricingStandard_Montly { get; set; }
        public decimal PricingNationalAccount_Daily { get; set; }
        public decimal PricingNationalAccount_Weekly { get; set; }
        public decimal PricingNationalAccount_Montly { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


    }
}
