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
    
    public partial class equipmentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public equipmentDetail()
        {
            this.feesDetails = new HashSet<feesDetail>();
        }
    
        public int id { get; set; }
        public Nullable<int> ReservationID { get; set; }
        public Nullable<int> modelGroup { get; set; }
        public Nullable<int> equipmentGroup { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> returnDate { get; set; }
        public Nullable<decimal> dailyRate { get; set; }
        public Nullable<decimal> weeklyRate { get; set; }
        public Nullable<decimal> monthlyRate { get; set; }
        public string transportation { get; set; }
    
        public virtual Forklift Forklift { get; set; }
        public virtual ForkliftsModel ForkliftsModel { get; set; }
        public virtual ReservationAPI ReservationAPI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<feesDetail> feesDetails { get; set; }
    }
}
