using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Print.Interface
{
    public interface ITraductionService
    {
        string Get(string culture, string word);
    }
}
