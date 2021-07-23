using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class SaunaDTO
    {
        public SaunaDTO(int saunaID, int establishmentID, string temperatureThreshold, string cO2Threshold, string humidityThreshold)
        {
            SaunaID = saunaID;
            EstablishmentID = establishmentID;
            TemperatureThreshold = temperatureThreshold;
            CO2Threshold = cO2Threshold;
            HumidityThreshold = humidityThreshold;
        }

        public SaunaDTO(int saunaID, int establishmentID, string temperatureThreshold, string cO2Threshold, string humidityThreshold, List<DatapointDTO> datapoints) : this(saunaID, establishmentID, temperatureThreshold, cO2Threshold, humidityThreshold)
        {
            Datapoints = datapoints;
        }

        public int SaunaID { get; set; }
        public int EstablishmentID { get; set; }
        public string TemperatureThreshold { get; set; }
        public string CO2Threshold { get; set; }
        public string HumidityThreshold { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DatapointDTO> Datapoints { get; set; }
    }
}