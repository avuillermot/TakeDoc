using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Print
{
    public class TraductionService : BaseService, Interface.ITraductionService
    {
        public string Get(string culture, string word)
        {
            if (word.ToUpper().Equals("TRUE")) return "Oui";
            if (word.ToUpper().Equals("FALSE")) return "Non";
            return word;
        }
    }
}
