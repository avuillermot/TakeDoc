using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.MyNHibernateHelper.Dao
{
    public interface IGenericDao<T>
    {
        ICollection<T> GetAll();
        ICollection<T> GetAll(string className);
        T GetById(string id);
        T GetById(string className, string id);
        void Update(T item);
        void Add(T item);
    }
}
