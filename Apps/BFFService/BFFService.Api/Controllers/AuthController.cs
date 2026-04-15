using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BFFService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("token")]
        public IActionResult GetDummyToken()
        {
            // 1. Get settings from appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? "Systel_Default_Secret_Key_32_Chars_Min";
            var issuer = jwtSettings["Issuer"] ?? "SystelBFF";
            var audience = jwtSettings["Audience"] ?? "SystelServices";

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // 2. Define user claims (you can add your name or ID here)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "RajArchitect"),
                new Claim(ClaimTypes.Role, "TechnicalArchitect"),
                new Claim("Project", "Systel")
            };

            // 3. Create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Valid for 2 hours
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 4. Return the token string
            return Ok(new
            {
                access_token = tokenHandler.WriteToken(token),
                expires_in = 7200,
                token_type = "Bearer"
            });
        }
    }
}