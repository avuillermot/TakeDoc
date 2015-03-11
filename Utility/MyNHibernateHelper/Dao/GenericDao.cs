using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Utility.MyNHibernateHelper.Dao
{
    public class GenericDao<T> : Base.ClassBase, IGenericDao<T>
    {
        public ISession CurrentSession
        {
            get
            {
                return Transaction.TransactionInterceptorConfiguration.SessionManager.CurrentSession;
            }
        }

        /// <summary>
        /// retourne la requete de base pour interrogé la table (=> from xxxx)
        /// </summary>
        /// <returns></returns>
        protected string GetBaseQuery()
        {
            return string.Format("from {0}", this.GetModelClassName());
        }

        protected string GetBaseQuery(string className)
        {
            return string.Format("from {0}", className);
        }

        protected string GetModelClassName()
        {
            return this.GetType().Name.Replace("Dao", "");
        }

        public ICollection<T> GetAll()
        {
            IQuery query = this.CurrentSession.CreateQuery(this.GetBaseQuery());
            ICollection<T> retour = query.List<T>();
            return retour;
        }

        public ICollection<T> GetAll(string className)
        {
            IQuery query = this.CurrentSession.CreateQuery(this.GetBaseQuery(className));
            ICollection<T> retour = query.List<T>();
            return retour;
        }

        public T GetById(string id)
        {
            string requete = string.Format("{0} where Id = '{1}'", this.GetBaseQuery(), id.ToString());
            IQuery query = this.CurrentSession.CreateQuery(requete);
            return this.GetExecById(requete);
        }

        public T GetById(string className, string id)
        {
            string requete = string.Format("{0} where Id = '{1}'", this.GetBaseQuery(className), id.ToString());
            return this.GetExecById(requete);
        }

        public T GetByProperty(string className, string property, string value)
        {
            string requete = string.Format("{0} where {1} = '{2}'", this.GetBaseQuery(className), property, value);
            return this.GetExecById(requete);
        }

        public T GetByProperty(string className, string property, long value)
        {
            string requete = string.Format("{0} where {1} = {2}", this.GetBaseQuery(className), property, value);
            return this.GetExecById(requete);
        }

        public T GetByProperty(string className, string property, int value)
        {
            string requete = string.Format("{0} where {1} = {2}", this.GetBaseQuery(className), property, value);
            return this.GetExecById(requete);
        }

        private T GetExecById(string requete)
        {
            IQuery query = this.CurrentSession.CreateQuery(requete);
            ICollection<T> lst = query.List<T>();
            if (lst.Count() == 0) return default(T);
            IEnumerable<T> enumerator = lst.Where<T>(item => item.GetType().Equals(typeof(T)));
            return enumerator.First<T>();
        }

        public ICollection<T> GetByIds(string ids)
        {
            string requete = string.Format("{0} where Id in ({1})", this.GetBaseQuery(), ids.ToString());
            IQuery query = this.CurrentSession.CreateQuery(requete);
            ICollection<T> lst = query.List<T>();
            return lst;
        }

        public void Update(T item)
        {
            this.CurrentSession.Update(item);
        }

        public void Add(T item)
        {
            this.CurrentSession.Save(item);
        }
    }
}
