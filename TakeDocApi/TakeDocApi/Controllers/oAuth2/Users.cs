using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using System.Security.Claims;

namespace TakeDocApi.Controllers.oAuth2
{
    public static class Users
    {
        public static ICollection<InMemoryUser> Get()
        {
            ICollection<InMemoryUser> retour = new List<InMemoryUser>();
            InMemoryUser u1 = new InMemoryUser() { Username = "leo", Password = "pwd", Subject = "my leo" };
            u1.Claims = new[]{
                new Claim("GvienName","Eleonore")
            };
            retour.Add(u1);
            return retour;
        }
    }
}