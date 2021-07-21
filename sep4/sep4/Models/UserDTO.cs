using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class UserDTO
    {
        public UserDTO(int userID, string username, string password, string rights)
        {
            UserID = userID;
            Username = username;
            Password = password;
            Rights = rights;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rights { get; set; }
    }
}