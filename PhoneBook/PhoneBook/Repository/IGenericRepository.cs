using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Models;

namespace PhoneBook.Repository
{
    public interface IGenericRepository <C> where C :  BaseEntity
    {
        void Insert(C obj);
        void Update(C obj);
        void Delete(C obj);
        IEnumerable<C> SelectAll();
        C SelectRow(int ID);
    }
}
