using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security
{
    public class TokenService : BaseService, Interface.ITokenService
    {
        private TakeDocDataAccess.DaoBase<TakeDocModel.RefreshToken> daoRefreshToken = new TakeDocDataAccess.DaoBase<TakeDocModel.RefreshToken>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.AccessToken> daoAccessToken = new TakeDocDataAccess.DaoBase<TakeDocModel.AccessToken>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();
        
        private TakeDocDataAccess.Security.Interface.IDaoUserTk daoUserTk = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();

        public TakeDocModel.RefreshToken CreateRefreshToken(Guid clientId, string source)
        {
            int durationRefresh = Convert.ToInt32(daoParameter.GetBy(x => x.ParameterReference == "REFRESH_TOKEN_DURATION").First().ParameterValue);
            
            TakeDocModel.RefreshToken refresh = new TakeDocModel.RefreshToken();
            TakeDocModel.UserTk user = daoUserTk.GetBy(x => x.UserTkId == clientId).First();

            refresh.Id = System.Guid.NewGuid();
            refresh.ClientId = clientId;
            refresh.DateStartUTC = System.DateTime.UtcNow;
            refresh.DateEndUTC = System.DateTime.UtcNow.AddSeconds(durationRefresh);
            refresh.Source = source;
              refresh.Role = user.GroupTk.GroupTkId;
            
            daoRefreshToken.Context.RefreshToken.Add(refresh);
            daoRefreshToken.Context.SaveChanges();

            this.GetNewAccessToken(clientId, refresh.Id, refresh.Role, refresh.Id, source);

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
                return this.GetNewAccessToken(refreshToken.ClientId, System.Guid.NewGuid(), refreshToken.Role, refreshTokenId, refreshToken.Source);
            }
            else
            {
                return accessTokens.First();
            }
        }

        public bool IsValidAccessToken(Guid accessTokenId, string[] accessGroup)
        {
            base.Logger.DebugFormat("Test access token {0} date", accessTokenId);
            // test if token is valid
            ICollection<TakeDocModel.AccessToken> accessTokens = daoAccessToken.GetBy(x => x.Id == accessTokenId
                && x.DateStartUTC <= System.DateTime.UtcNow
                && System.DateTime.UtcNow <= x.DateEndUTC);
            if (accessTokens.Count() > 1) return false;
            if (accessTokens.Count() <= 0) return false;
            base.Logger.DebugFormat("Test access token  {0} has valid date", accessTokenId);
            base.Logger.DebugFormat("Test access token  {0} group", accessTokenId);
            if (accessGroup.Count() == 1 && accessGroup[0] == string.Empty) return true;

            TakeDocModel.AccessToken current = accessTokens.First();
            TakeDocModel.UserTk user = daoUserTk.GetBy(x => x.UserTkId == current.ClientId, x => x.GroupTk).First();
            if (accessGroup.Contains(user.GroupTk.GroupTkReference) == false) {
                base.Logger.DebugFormat("no access for {0} token because is invalid group access", accessTokenId);
                return false;
            }
            base.Logger.DebugFormat("Test access token  {0} group is valid", accessTokenId);

            return true;
        }

        private TakeDocModel.AccessToken GetNewAccessToken(Guid clientId, Guid tokenId, Guid roleId, Guid refreshTokenId, string source)
        {
            int durationAccess = Convert.ToInt32(daoParameter.GetBy(x => x.ParameterReference == "ACCESS_TOKEN_DURATION").First().ParameterValue);
            TakeDocModel.AccessToken access = new TakeDocModel.AccessToken();

            access.Id = tokenId;
            access.ClientId = clientId;
            access.DateStartUTC = System.DateTime.UtcNow;
            access.DateEndUTC = System.DateTime.UtcNow.AddSeconds(durationAccess);
            access.Source = source;
            access.Role = roleId;
            access.RefreshTokenId = refreshTokenId;
            daoAccessToken.Context.SaveChanges();

            daoAccessToken.Context.AccessToken.Add(access);
            daoRefreshToken.Context.SaveChanges();
            return access;
        }
    }
}
