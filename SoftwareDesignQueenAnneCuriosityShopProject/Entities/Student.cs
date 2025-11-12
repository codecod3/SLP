using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LRN { get; set; }
        public bool EnrollmentStatus { get; set; }
        public string PhoneNumber { get; set; }
        // Relationships

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Relationship> Relationships { get; set; }
        public int? AdvisoryID { get; set; }
        public Advisory AdvisoryLink { get; set; }

    }
}
