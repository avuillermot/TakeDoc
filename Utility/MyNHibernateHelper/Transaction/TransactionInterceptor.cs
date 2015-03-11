using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Utility.MyNHibernateHelper.Transaction
{
    public static class TransactionInterceptorConfiguration {
        public static NHibernate.Cfg.Configuration Configuration { get; set; }
        public static Utility.MyNHibernateHelper.SessionManager SessionManager { get; set; }
    }
    public class TransactionInterceptor : IInterceptionBehavior
    {
        IEnumerable<Type> IInterceptionBehavior.GetRequiredInterfaces()
        {
            return new HashSet<Type>();
        }

        IMethodReturn IInterceptionBehavior.Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn retour = null;

            TransactionalAttribute transactional = null;
            object[] atts = input.MethodBase.GetCustomAttributes(typeof(TransactionalAttribute), false);
            if (atts.Count() > 0) transactional = (TransactionalAttribute) atts[0];

            if (Utility.MyNHibernateHelper.SessionManager.Configuration == null)
            {
                SessionManager.Configuration = TransactionInterceptorConfiguration.Configuration;
                TransactionInterceptorConfiguration.SessionManager = new SessionManager();
            }

            if (NHibernate.Context.CurrentSessionContext.HasBind(SessionManager.SessionFactory) == false)
                NHibernate.Context.CurrentSessionContext.Bind(SessionManager.SessionFactory.OpenSession());

            NHibernate.ISession session = null;
            NHibernate.ITransaction transaction = null;
            if (transactional != null)
            {
                session = TransactionInterceptorConfiguration.SessionManager.CurrentSession;
                if (transactional.OpenTransaction) transaction = session.BeginTransaction();
            }
               
            try
            {
                retour = getNext()(input, getNext);
                if (transaction != null && transactional.RollBack == true)
                {
                    transaction.Rollback();
                    //if (typeof(retour.Exception) == typeof(NHibernate.Exceptions.GenericADOException)) retour.Exception = null;
                }
                else if (retour.Exception == null && transaction != null) transaction.Commit();
                else if (retour.Exception != null) throw retour.Exception;
            }
            catch (Exception ex)
            {
                if (transaction != null) transaction.Rollback();
                else throw ex;
            }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (session != null)
                {
                    session.Close();
                    session.Dispose();
                    NHibernate.Context.CurrentSessionContext.Unbind(SessionManager.SessionFactory);
                }
            }
            
            return retour;
        }

        bool IInterceptionBehavior.WillExecute
        {
            get {
                return true;
            }
        }

    }
}
