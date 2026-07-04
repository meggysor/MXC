using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MXC.Extension;
using MXC.Models.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MXC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfigurationGetter _configurationGetter;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(ILogger<AccountController> logger, IConfigurationGetter configurationGetter, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _configurationGetter = configurationGetter;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt with invalid model state.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.PassWord))
            {
                _logger.LogWarning("Login attempt failed for email: {Email}", loginModel.Email);
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginModel.PassWord);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login attempt failed for email: {Email}", loginModel.Email);
                return Unauthorized("Invalid email or password.");
            }

            var authResponse = new AuthResponse
            {
                Token = GenerateJwtToken(user),
                Email = loginModel.Email
            };

            _logger.LogInformation("User {Email} logged in successfully.", loginModel.Email);
            return Ok(authResponse);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configurationGetter.JWTKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configurationGetter.JWTIssuer,
                audience: _configurationGetter.JWTAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_configurationGetter.JwtExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
