//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReservationHdr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReservationHdr()
        {
            this.ReservationDtls = new HashSet<ReservationDtl>();
        }
    
        public int id { get; set; }
        public string DocumentNumber { get; set; }
        public int customerNumber { get; set; }
        public string CustomerPO { get; set; }
        public string Comments { get; set; }
        public decimal TotalRentAmount { get; set; }
        public string Contract { get; set; }
        public string Insurance { get; set; }
        public int ReservationStatusId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDtl> ReservationDtls { get; set; }
        public virtual ReservationStatu ReservationStatu { get; set; }
        public virtual User User { get; set; }
    }
}