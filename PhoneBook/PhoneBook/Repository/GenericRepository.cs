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
    public class GenericRepository<C> : IGenericRepository<C> where C : BaseEntity
    {
        private readonly PhoneBookContext context;
        private DbSet<C> entity;

        public GenericRepository(PhoneBookContext context)
        {
            this.context = context;
            entity = context.Set<C>();
        }
        public void Delete(C obj)
        {
            entity.Remove(obj);
        }

        public void Insert(C obj)
        {
            entity.Add(obj);
        }

        public IEnumerable<C> SelectAll()
        {
            return entity.ToList();
        }

        public C SelectRow(int ID)
        {
            return entity.Find(ID);
        }

        public void Update(C obj)
        {
            entity.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
        }
    }
}
