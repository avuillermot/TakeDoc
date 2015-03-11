using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Base
{
    public interface IClassBase
    {
        bool hasError { get; set; }
        string messageError { get; set; }
    }
    public abstract class ClassBase : IClassBase
    {
        public virtual bool hasError { get; set; }
        public virtual string messageError { get; set; } 

        public virtual void clearError()
        {
            this.hasError = false;
            this.messageError = String.Empty;
        }

        public virtual void setError(string message)
        {
            this.hasError = true;
            this.messageError = message;

            Logger.myLogger.Error(message);
        }

        public virtual void setError(Exception ex)
        {
            this.hasError = true;
            if (ex.InnerException != null) this.messageError = ex.InnerException.Message;
            else this.messageError = ex.Message;

            Logger.myLogger.Error(ex);
        }

        public virtual void logStart(string functionName, string msg) {
            this.logPoint("START", functionName, msg);
        }

        public virtual void logEnd(string functionName, string msg)
        {
            this.logPoint("END",functionName,msg);
        }

        public virtual void logStep(string step, string functionName, string msg)
        {
            this.logPoint(step, functionName, msg);
        }

        private void logPoint(string step, string functionName, string msg)
        {
            Logger.myLogger.Debug(string.Format("[{0}] {1} - {2}",step, functionName, msg));
        }
    }
}
