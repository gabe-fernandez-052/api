using JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(IConfiguration config) : Controller
    {
        [HttpPost("token/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        public IActionResult GenerateToken(string clientId)
        {
            var response = new Response<string>();
            var user = config.GetSection("Users").Get<List<User>>()?.SingleOrDefault(x => x.ClientId == clientId);

            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                claims.Add(new Claim(ClaimTypes.Sid, user.ClientId.ToString()));

                var token = new JwtSecurityToken(config["Jwt:Issuer"],
                                                 config["Jwt:Audience"],
                                                 claims,
                                                 expires: DateTime.Now.AddHours(1),
                                                 signingCredentials: credentials);

                response.Content = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(response);
            }

            response.Error = "Client id not found";

            return NotFound(response);
        }
    }
}