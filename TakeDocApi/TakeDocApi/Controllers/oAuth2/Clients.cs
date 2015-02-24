using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;

namespace TakeDocApi.Controllers.oAuth2
{
    public static class Clients
    {
        public static ICollection<Client> Get()
        {
            ICollection<Client> retour = new List<Client>();
            Client client = new Client() { 
                ClientName = "Rouhana Eleonore",
                ClientId = "1",
                Enabled = true,
                AccessTokenType = AccessTokenType.Reference,

                Flow = Flows.ClientCredentials,
                ClientSecrets = new List<ClientSecret> {
                    new ClientSecret("test_by_avt".Sha256())
                },
                RedirectUris = new List<string>
                {
                    "http://www.lequipe.fr"
                }
            };

            retour.Add(client);
            return retour;
        }
    }
}
