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
    
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FCMToken { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsViewed { get; set; }
    
        public virtual User User { get; set; }
    }
}