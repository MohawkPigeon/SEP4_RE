//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sep4
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int UserID { get; set; }
        public int SaunaID { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public System.DateTime ToDateTime { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
    
        public virtual Sauna Sauna { get; set; }
        public virtual User User { get; set; }
    }
}
