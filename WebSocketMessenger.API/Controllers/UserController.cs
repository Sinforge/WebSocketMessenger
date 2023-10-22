using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebSockerMessenger.Core.Configuration;
using WebSockerMessenger.Core.DTOs;
using WebSockerMessenger.Core.Interfaces.Services;
using WebSockerMessenger.Core.Models;

namespace WebSocketMessenger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<Audience> _options;
        public UserController(IUserService userService, IOptions<Audience> options) {
            _options = options;
            _userService = userService;
        }


        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([FromBody] UserDTO userDTO)
        {
            if (await _userService.CreateUserAsync(new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Surname = userDTO.Surname,
                Name = userDTO.Name,
            
            })) return StatusCode(201);
            else return StatusCode(400);
        }

        [HttpGet]
        [Route("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authorize([FromBody] LoginDTO loginDTO)
        {
            User? user = await _userService.CheckUserCredentials(loginDTO.Login, loginDTO.Password);
            if (user != null)
            {
                JwtResponse token = CreateToken(user);
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }

        }
        [NonAction]
        private JwtResponse CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            };
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = _options.Value.Iss,
                ValidateAudience = true,
                ValidAudience = _options.Value.Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Value.Iss,
                audience: _options.Value.Aud,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new JwtResponse(encodedJWT, (int)TimeSpan.FromMinutes(2).TotalSeconds);
            return response;
        }
        [NonController]
        private record JwtResponse(string? access_token, int expires_in);



    }
}
