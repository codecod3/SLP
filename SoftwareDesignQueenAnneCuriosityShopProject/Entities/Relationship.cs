using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Relationship
    {
        public int RelationshipID { get; set; }
        public RelationshipType TypeOfRelationship { get; set; }


        //relationship
        public int? StudentID { get; set; }
        public Student StudentLink { get; set; }

        public int? ParentID { get; set; }
        public Parent ParentLink { get; set; }
    }

    public enum RelationshipType
    {
        Father,
        Mother,
        Guardian,
        Other
    }
}
