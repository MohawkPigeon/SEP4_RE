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
    
    public partial class StageReservationDim
    {
        public int Id { get; set; }
        public Nullable<int> UserID { get; set; }
        public string SaunaID { get; set; }
        public Nullable<System.DateTime> FromDateTime { get; set; }
        public Nullable<System.DateTime> ToDateTime { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
    }
}