using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class SaunaDTO
    {
        public SaunaDTO(int saunaID, int establishmentID, string threshold)
        {
            SaunaID = saunaID;
            EstablishmentID = establishmentID;
            Threshold = threshold;
        }

        public SaunaDTO(int saunaID, int establishmentID, string threshold, List<DatapointDTO> datapoints)
        {
            SaunaID = saunaID;
            EstablishmentID = establishmentID;
            Threshold = threshold;
            Datapoints = datapoints;
        }

        public int SaunaID { get; set; }
        public int EstablishmentID { get; set; }
        public string Threshold { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DatapointDTO> Datapoints { get; set; }
    }
}