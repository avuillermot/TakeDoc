using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.MyNHibernateHelper.Transaction
{
    public class TransactionalAttribute : Attribute
    {
        private bool _rollback = false;
        public bool RollBack {
            get
            {
                return _rollback;
            }
            set
            {
                _rollback = value;
            }
        }

        private bool _openTransaction = false;
        public bool OpenTransaction
        {
            get
            {
                return _openTransaction;
            }
            set
            {
                _openTransaction = value;
            }
        }

        public TransactionalAttribute(bool OpenTransaction, bool RollBack)
        {
            this.OpenTransaction = OpenTransaction;
            this.RollBack = RollBack;
        }
    }
}
