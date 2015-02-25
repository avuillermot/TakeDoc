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
            List<Scope> scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                new Scope
                {
                    Enabled = true,
                    Name = "sampleApi",
                    Description = "Access to a sample API",
                    Type = ScopeType.Resource
                }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}
