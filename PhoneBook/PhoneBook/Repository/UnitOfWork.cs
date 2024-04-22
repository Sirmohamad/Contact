using PhoneBook.Forms.Context;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Repository
{
    public class UnitOfWork : IDisposable
    {
        PhoneBookContext db = new PhoneBookContext();

        private IContactRepository _contactRepository;
        public IContactRepository contactRepository
        {
            get
            {
                if(_contactRepository == null)
                {
                    _contactRepository = new ContactRepository(db);
                }

                return _contactRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
