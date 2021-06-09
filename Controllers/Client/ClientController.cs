using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wuther.IdentityServer.Controllers.Client
{
    public class ClientController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IConfiguration _configuration;
        //private readonly IConfigurationDbContext _configurationDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly IEventService _events;

        public ClientController(IClientStore clientStore, 
            IConfiguration Configuration, 
            ConfigurationDbContext configurationDbContext,
            IEventService events)
        {
            _clientStore = clientStore;
            _configuration = Configuration;
            _configurationDbContext = configurationDbContext;
            _events = events;
        }
        public IActionResult Index()
        {
            var clientInput = new ClientInputModel();
            return View(clientInput);
        }

        public IActionResult Success(IdentityServer4.Models.Client client)
        {
            return View(client);
        }

        public async Task<IActionResult> Add(ClientInputModel model)
        {
            var secret = new Guid().ToString();
            var client = new IdentityServer4.Models.Client()
            {
                ClientId = model.ClientId,
                ClientName = model.ClientName,
                AllowedGrantTypes = new string[1] { model.AllowedGrantTypes },
                ClientSecrets = { new Secret(secret.Sha256()) },
                AllowedScopes = model.AllowedScopes
            };
            await _configurationDbContext.Clients.AddAsync(client.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            if(result > 0)
            {
                return RedirectToAction("Success", client);
            }
            ModelState.AddModelError(string.Empty, "create error");
            return View();
        }
    }
}
