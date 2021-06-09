using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wuther.IdentityServer.Controllers.Client
{
    public class ClientInputModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string AllowedGrantTypes { get; set; }
        public string[] AllowedScopes { get; set; }
    }
}
