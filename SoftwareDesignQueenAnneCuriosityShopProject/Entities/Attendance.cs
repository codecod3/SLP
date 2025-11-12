using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public DateTime Date { get; set; }
        public bool isPresent { get; set; }

        // Relationships
        public int? StudentID { get; set; }
        public Student StudentLink { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
