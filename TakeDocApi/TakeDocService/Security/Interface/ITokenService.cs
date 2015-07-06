using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security.Interface
{
    public interface ITokenService
    {
        TakeDocModel.RefreshToken CreateRefreshToken(Guid userTkId, string source, string clientId);
        TakeDocModel.AccessToken GetAccessToken(Guid refreshTokenId);
    }
}
