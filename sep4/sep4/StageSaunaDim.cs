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
    
    public partial class StageSaunaDim
    {
        public int Id { get; set; }
        public Nullable<int> SaunaID { get; set; }
        public Nullable<int> EstablishmentID { get; set; }
        public string NameOrNumber { get; set; }
        public string TemperatureThreshold { get; set; }
        public string CO2Threshold { get; set; }
        public string HumidityThreshold { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
    }
}
