using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class EstablishmentDTO
    {
        public EstablishmentDTO(int establishmentID, string name)
        {
            EstablishmentID = establishmentID;
            Name = name;
        }

        public int EstablishmentID { get; set; }
        public string Name { get; set; }
    }
}