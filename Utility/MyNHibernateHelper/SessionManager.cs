using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using nhcfg = NHibernate.Cfg;

namespace Utility.MyNHibernateHelper
{
    public class SessionManager : IDisposable
    {
        public static nhcfg.Configuration Configuration { get; set; }
        public static ISessionFactory SessionFactory { get; set; }

        public SessionManager()
        {
            this.Init();
        }

        private void Init()
        {
            if (SessionFactory == null)
            {
                if (Configuration == null) SessionFactory = new nhcfg.Configuration().Configure().BuildSessionFactory();
                else SessionFactory = Configuration.Configure().BuildSessionFactory();
            }
        }

        public ISession CurrentSession
        {
            get
            {
                Logger.myLogger.Debug("Utilisation d'une session NHibernate existante.");
                try
                {
                    ISession session = SessionFactory.GetCurrentSession();
                    return session;
                 }
                catch (HibernateException ex)
                {
                    Logger.myLogger.Error(ex);
                    return null;
                }
            }
        }

        public void Dispose()
        {
            NHibernate.Context.CurrentSessionContext.Unbind(SessionFactory);
            SessionFactory.Dispose();
        }
    }
}
