using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Models;


namespace PhoneBook.Forms.Context
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext() : base("PhoneBook_DBEntities")
        {

        }



        public DbSet<Contact> Contacts { get; set; }

    }
}
