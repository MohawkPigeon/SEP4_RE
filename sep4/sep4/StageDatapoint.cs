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
    
    public partial class StageDatapoint
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> Temperature { get; set; }
        public Nullable<int> CO2 { get; set; }
        public Nullable<int> Humidity { get; set; }
        public string ServoSettingAtTime { get; set; }
    }
}