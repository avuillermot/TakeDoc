﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TakeDocWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            TakeDocDataAccess.DaoBase<TakeDocModel.UserTk> daoUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.DaoBase<TakeDocModel.UserTk>>();
            TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkLastName == "SYSTEM").First();
            Utility.Logger.myLogger.Init();
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            // use root id
            Console.WriteLine("fin");
        }
    }
}
