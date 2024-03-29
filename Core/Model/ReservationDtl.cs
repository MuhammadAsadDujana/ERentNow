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
    
    public partial class ReservationDtl
    {
        public int Id { get; set; }
        public int ReservationID { get; set; }
        public int ForkliftsModelId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string Location { get; set; }
        public decimal RentPrice { get; set; }
    
        public virtual ForkliftsModel ForkliftsModel { get; set; }
        public virtual ReservationHdr ReservationHdr { get; set; }
    }
}
