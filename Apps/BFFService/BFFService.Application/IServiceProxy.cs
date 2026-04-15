using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFFService.Application
{
    public interface IServiceProxy
    {
        Task<HttpResponseMessage> SendAsync(string clientName, HttpRequestMessage request);
    }
}
