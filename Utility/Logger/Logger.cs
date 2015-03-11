using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace Utility.Logger
{
    public static class myLogger
    {
        private static bool isInit = false;
        
        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            isInit = true;
        }
        public static void Debug(string message)
        {
            if (isInit == false) Init();

            LogManager.GetLogger("myFileAppenderDebug").Debug(message);
        }
        public static void Info(string message)
        {
            if (isInit == false) Init();
            LogManager.GetLogger("myFileAppenderInfo").Info(message);
        }
        public static void Error(string message)
        {
            if (isInit == false) Init();

            LogManager.GetLogger("myFileAppenderError").Error(message);
        }
        public static void Console(string message)
        {
            if (isInit == false) Init();
            LogManager.GetLogger("myConsoleAppender").Debug(message);
        }
        public static ILog getLogger(string loggerName)
        {
            if (isInit == false) Init();

            return LogManager.GetLogger(loggerName);
        }

        public static void Error(Exception ex)
        {
            if (isInit == false) Init();
            if (ex.Message != null) 
                LogManager.GetLogger("myFileAppenderError").Error(ex.Message);
            if (ex.InnerException != null)
                LogManager.GetLogger("myFileAppenderError").Error(ex.InnerException);
            if (ex.StackTrace != null)
                LogManager.GetLogger("myFileAppenderError").Error(ex.StackTrace);
        }
    }
}
