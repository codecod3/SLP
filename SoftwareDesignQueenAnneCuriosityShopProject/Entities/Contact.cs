using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Contact
    {
        public int ContactID { get; set; }
        public string PhoneNumber { get; set; }
        public string Network { get; set;  }


        //relationship
        public int? ParentID { get; set; }
        public Parent ParentLink { get; set; }

        public ICollection<Delivered> DeliveredMsg { get; set; }

    }
}
