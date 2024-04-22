using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Repository
{
    public interface IContactRepository : IGenericRepository<Contact> 
    {
        bool PhoneNumberExists(string phoneNumber);
        IEnumerable<Contact> SearchContacts(string name, string mobile, DateTime? fromDate, DateTime? toDate);
    }
}
