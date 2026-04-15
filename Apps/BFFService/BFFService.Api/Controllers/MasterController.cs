using BFFService.Application;
using Microsoft.AspNetCore.Mvc;


namespace BFFService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterController : Controller
    {
        private readonly IServiceProxy _serviceProxy;
        private const string ClientName = "PartServiceClient";

        public MasterController(IServiceProxy serviceProxy)
        {
            _serviceProxy = serviceProxy;
        }

        [NonAction]
        public IActionResult Index()
        {
            return View();
        }


        // GET: api/master/parts
        [HttpGet("parts")]
        public async Task<IActionResult> GetAllParts()
        {
            // 1. Log the incoming request at the BFF level
            //_logger.LogInformation("BFF: Forwarding request to fetch all parts from MasterService.");

            // 2. Create the request to the MasterService endpoint
            // Note: The path should match the route in your MasterService (api/parts)
            var request = new HttpRequestMessage(HttpMethod.Get, "api/parts");

            // 3. Send via the proxy
            var response = await _serviceProxy.SendAsync(ClientName, request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }

            // 4. Handle errors gracefully
            //_logger.LogError("BFF: Failed to retrieve parts from MasterService. Status: {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, "Unable to fetch parts at this time.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMasterData(int id)
        {
            // 1. Create the request to the internal microservice
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/master/{id}");

            // 2. Use the proxy to forward the call (it handles the JWT automatically)
            var response = await _serviceProxy.SendAsync(ClientName, request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, "Error calling MasterService");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMasterRecord([FromBody] object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/master")
            {
                Content = JsonContent.Create(data)
            };

            var response = await _serviceProxy.SendAsync(ClientName, request);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }

            return StatusCode((int)response.StatusCode);
        }
    }
}
