using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }


        //relationship
        public int? AttendanceID { get; set;}
        public Attendance AttendanceLink { get; set; }

        public ICollection<Delivered> DeliveredMsg { get; set; }
    }
}
