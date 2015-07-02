using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel.Dto.Document
{
    public class SearchMetadata : TakeDocModel.MetaData
    {
        public string Condition;
        public SearchMetadata(string condition)
        {
            Condition = condition;
        }
    }

    public static class SearchCondition
    {
        public static string Equals
        {
            get
            {
                return "=";
            }
        }

        public static string Greater
        {
            get
            {
                return ">";
            }
        }

        public static string Less
        {
            get
            {
                return "<";
            }
        }

        public static string Start
        {
            get
            {
                return "START";
            }
        }

        public static string Contains
        {
            get
            {
                return "CONTAINS";
            }
        }
    }
}
