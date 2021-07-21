using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class NotificationHistoryDTO
    {
        public NotificationHistoryDTO(int notificationID, int userID, DateTime? dateTime)
        {
            NotificationID = notificationID;
            UserID = userID;
            DateTime = dateTime;
        }

        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
    }
}