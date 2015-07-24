using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class Contact
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneMobile { get; set; }
        
        public bool UserLinked { get; set; }
        public bool Disabled { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneHome { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public Guid Uuid { get; set; }
        public int UserId { get; set; }

    }
}
