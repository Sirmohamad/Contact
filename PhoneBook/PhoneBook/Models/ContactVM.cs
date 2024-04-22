using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class ContactVM : BaseEntity
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Mobile { get; set; }
        public string BirthDate { get; set; }
        public byte[] Avatar { get; set; }
        public string Address { get; set; }
    }
}
