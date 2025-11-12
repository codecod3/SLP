using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Advisory
    {
        public int AdvisoryID { get; set; }
        public string Name { get; set; }
        public string SectionName { get; set; }

        public string SchoolYear { get; set; }

        // Relationships
        public int? ClassAdviserID { get; set; }
        public ClassAdviser ClassAdviserLink { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
