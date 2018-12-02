using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Data.Entities;
using TasksManagement.Models;

namespace TasksManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly IConfiguration _configuration;
        readonly ILogger<TokenController> _logger;

        public TokenController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ILogger<TokenController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok("ok");
        }



        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, isPersistent: false, lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                {
                    return BadRequest(new { message = "invalid username or password" });

                }

                var user = await _userManager.FindByNameAsync(loginModel.Username);

                return Ok(
                    new AuthUserModel()
                    {
                        UserID = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Token = GenerateToken(user)
                    }
                );
            }
            return BadRequest(ModelState);

        }

        [Authorize]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey(JwtRegisteredClaimNames.UniqueName)).Select(c => c.Value).FirstOrDefault()
                );
            return Ok(GenerateToken(user));

        }

        private string GenerateToken(User user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Jwt:Key")));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Jwt:Lifetime")),
                audience: _configuration.GetValue<String>("Jwt:Audience"),
                issuer: _configuration.GetValue<String>("Jwt:Issuer")
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

    }
}