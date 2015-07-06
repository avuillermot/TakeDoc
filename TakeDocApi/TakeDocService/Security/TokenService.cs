using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security
{
    public class TokenService : Interface.ITokenService
    {
        private TakeDocDataAccess.DaoBase<TakeDocModel.RefreshToken> daoRefreshToken = new TakeDocDataAccess.DaoBase<TakeDocModel.RefreshToken>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.AccessToken> daoAccessToken = new TakeDocDataAccess.DaoBase<TakeDocModel.AccessToken>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();
        
        private TakeDocDataAccess.Security.Interface.IDaoUserTk daoUserTk = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();

        public TakeDocModel.RefreshToken CreateRefreshToken(Guid userTkId, string source, string clientId)
        {
            int durationRefresh = Convert.ToInt32(daoParameter.GetBy(x => x.ParameterReference == "REFRESH_TOKEN_DURATION").First().ParameterValue);
            
            TakeDocModel.RefreshToken refresh = new TakeDocModel.RefreshToken();
            TakeDocModel.UserTk user = daoUserTk.GetBy(x => x.UserTkId == userTkId).First();

            refresh.Id = System.Guid.NewGuid();
            refresh.ClientId = clientId;
            refresh.DateStartUTC = System.DateTime.UtcNow;
            refresh.DateEndUTC = System.DateTime.UtcNow.AddSeconds(durationRefresh);
            refresh.Source = source;
            refresh.UserTkId = userTkId;
            refresh.Role = user.GroupTk.GroupTkId;
            
            daoRefreshToken.Context.RefreshToken.Add(refresh);
            daoRefreshToken.Context.SaveChanges();

            this.GetNewAccessToken(userTkId, refresh.Id, refresh.Role, refresh.Id, source, clientId);

            return refresh;
        }

        public TakeDocModel.AccessToken GetAccessToken(Guid refreshTokenId)
        {

            ICollection<TakeDocModel.AccessToken> accessTokens = daoAccessToken.GetBy(x => x.RefreshTokenId == refreshTokenId
                && x.DateStartUTC <= System.DateTime.UtcNow
                && System.DateTime.UtcNow <= x.DateEndUTC);

            if (accessTokens.Count() == 0)
            {
                TakeDocModel.RefreshToken refreshToken = daoRefreshToken.GetBy(x => x.Id == refreshTokenId).First();
                return this.GetNewAccessToken(refreshToken.UserTkId, System.Guid.NewGuid(), refreshToken.Role, refreshTokenId, refreshToken.Source, refreshToken.ClientId);
            }
            else
            {
                return accessTokens.First();
            }
        }

        private TakeDocModel.AccessToken GetNewAccessToken(Guid userTkId, Guid tokenId, Guid roleId, Guid refreshTokenId, string source, string clientId)
        {
            int durationAccess = Convert.ToInt32(daoParameter.GetBy(x => x.ParameterReference == "ACCESS_TOKEN_DURATION").First().ParameterValue);
            TakeDocModel.AccessToken access = new TakeDocModel.AccessToken();

            access.Id = tokenId;
            access.ClientId = clientId;
            access.DateStartUTC = System.DateTime.UtcNow;
            access.DateEndUTC = System.DateTime.UtcNow.AddSeconds(durationAccess);
            access.Source = source;
            access.UserTkId = userTkId;
            access.Role = roleId;
            access.RefreshTokenId = refreshTokenId;
            daoAccessToken.Context.SaveChanges();

            daoAccessToken.Context.AccessToken.Add(access);
            daoRefreshToken.Context.SaveChanges();
            return access;
        }
    }
}
