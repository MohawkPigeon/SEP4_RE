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
    
    public partial class UserDim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDim()
        {
            this.SaunaFact = new HashSet<SaunaFact>();
        }
    
        public int UserDimID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Rights { get; set; }
        public System.DateTime ActiveSince { get; set; }
        public System.DateTime LoadDate { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaunaFact> SaunaFact { get; set; }
    }
}
