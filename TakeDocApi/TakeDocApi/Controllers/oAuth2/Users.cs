using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using Thinktecture.IdentityServer.Core.Services.Default;
using System.Security.Claims;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;

namespace TakeDocApi.Controllers.oAuth2
{
    public static class Users
    {
        private static IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
        public static ICollection<InMemoryUser> Get()
        {
            ICollection<InMemoryUser> retour = new List<InMemoryUser>();
            ICollection<TakeDocModel.UserTk> users = servUser.GetAll();
            foreach (TakeDocModel.UserTk user in users)
            {
                InMemoryUser u1 = new InMemoryUser() { Username = user.UserTkLogin, Password = user.UserTkPassword, Subject = String.Concat(user.UserTkFirstName," ", user.UserTkLastName) };
                u1.Claims = new[]{
                    new Claim("GvienName","Eleonore")
                };
                retour.Add(u1);
            }
           
            return retour;
        }
    }
}