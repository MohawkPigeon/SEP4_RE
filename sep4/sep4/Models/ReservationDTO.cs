using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class ReservationDTO
    {
        public ReservationDTO(int userID, int saunaID, DateTime? fromDateTime, DateTime? toDateTime)
        {
            UserID = userID;
            SaunaID = saunaID;
            FromDateTime = fromDateTime;
            ToDateTime = toDateTime;
        }

        public int UserID { get; set; }
        public int SaunaID { get; set; }
        public Nullable<System.DateTime> FromDateTime { get; set; }
        public Nullable<System.DateTime> ToDateTime { get; set; }
    }
}