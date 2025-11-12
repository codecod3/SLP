using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Entities
{
    public class Parent
    {
        public int ParentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }


        //relationship
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Relationship> Relationships { get; set; }

    }
}
