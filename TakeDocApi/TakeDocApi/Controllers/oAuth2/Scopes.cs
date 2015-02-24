using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Core.Models;


namespace TakeDocApi.Controllers.oAuth2
{
    public static class Scopes
    {
        public static ICollection<Scope> Get()
        {
            ICollection<Scope> retour = new List<Scope>();
            retour.Add(new Scope() { Name = "myApi1" });
            return retour;
        }
    }
}