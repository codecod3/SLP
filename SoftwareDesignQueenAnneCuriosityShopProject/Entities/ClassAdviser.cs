using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class ClassAdviser
    {
        public int ClassAdviserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        // Relationships
        public ICollection<Advisory> Advisories { get; set; }

    }
}
