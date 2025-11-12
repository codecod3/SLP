using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Delivered
    {

        public int DeliveredID { get; set; }
        public DateTime DateTimeSent { get; set; }

        //relationship
        public int? NotificationID { get; set; }
        public Notification NotificationLink { get; set; }

        public int? ContactID { get; set; }
        public Contact ContactLink { get; set; }



    }
}
