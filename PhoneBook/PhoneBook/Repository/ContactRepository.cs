using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Forms.Context;
using PhoneBook.Models;
using PhoneBook.Repository;

namespace PhoneBook.Repository
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private  DbSet<Contact> entity;

        public ContactRepository(PhoneBookContext context) : base(context)
        {
            entity = context.Set<Contact>();
        }

        public bool PhoneNumberExists(string phoneNumber)
        {
            return entity.Any(c => c.Mobile == phoneNumber);
        }

        public IEnumerable<Contact> SearchContacts(string name, string mobile, DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Contact> query = (IQueryable<Contact>)entity;


            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name) || c.Family.Contains(name));
            }


            if (!string.IsNullOrWhiteSpace(mobile))
            {
                query = query.Where(c => c.Mobile.Contains(mobile));
            }


            if (fromDate != null)
            {
                query = query.Where(c => c.BirthDate >= fromDate.Value);
            }


            if (toDate != null)
            {
                query = query.Where(c => c.BirthDate <= toDate.Value);
            }


            return query.ToList();
        }
    }
    
}
