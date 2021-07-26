using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models.Stage
{
    public class StageEstablishment
    {
        public StageEstablishment(int establishmentID, string name)
        {
            EstablishmentID = establishmentID;
            Name = name;
        }

        public int EstablishmentID { get; set; }
        public string Name { get; set; }


        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: Establishments
        public ActionResult Index()
        {
            foreach (var item in collection)
            {
                StageEstablishment stageEstablishment = new StageEstablishment()
            }
            View(db.Establishment.ToList());
            db.StageEstablishmentDIM
        }
    }
}