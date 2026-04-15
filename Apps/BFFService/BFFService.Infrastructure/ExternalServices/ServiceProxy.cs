using BFFService.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BFFService.Infrastructure.ExternalServices
{
    public class ServiceProxy : IServiceProxy
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceProxy(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SendAsync(string clientName, HttpRequestMessage request)
        {
            var client = _httpClientFactory.CreateClient(clientName);

            // 1. Safely check if HttpContext exists
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                // 2. Forward the JWT Token
                var token = await context.GetTokenAsync("access_token");

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            else
            {
                // Optional: Log a warning that you're trying to call a service without a user context
                // _logger.LogWarning("No HttpContext found while trying to forward token to {ClientName}", clientName);
            }
            return await client.SendAsync(request);
        }
    }
}
