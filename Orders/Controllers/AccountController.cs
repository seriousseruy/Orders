using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Orders.Utility;

namespace Orders.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [Authorize]
        [HttpGet]
        public void Login()
        {
           //Authorize via cookie scheme
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> GetToken(string access_token)
        {
            ClaimsPrincipal claimsPrincipal = ValidateToken(access_token);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = access_token
                }
            });

            await HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);

            return Redirect("/UserProfile");
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var securityTokenHandler = new JwtSecurityTokenHandler();

            return securityTokenHandler.ValidateToken(token, JwtTokenHandler.GetTokenValidationParameters(), out _);
        }
    }
}
