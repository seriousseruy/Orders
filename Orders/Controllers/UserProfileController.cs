using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Orders.ViewModels;

namespace Orders.Controllers
{
    [Route("[controller]")]
    public class UserProfileController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: UserProfile
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userProfileViewModel = await CreateUserProfileViewModel();

                return View("UserProfile", userProfileViewModel);
            }

            return Redirect("/Account/Login");
        }

        public async Task<UserProfileViewModel> CreateUserProfileViewModel()
        {
            var userContent = await RequestToEndpoint(_configuration["UserInformationEndpoint"]);
            var userInfo = JObject.Parse(userContent);

            var firstName = userInfo.SelectToken("FirstName").Value<string>();
            var lastName = userInfo.SelectToken("LastName").Value<string>();
            var email = userInfo.SelectToken("MailAddress").Value<string>();
            var avatarUri = userInfo.SelectToken("AvatarSasUrl").Value<string>();

            var roles = new List<string>();

            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                roles.AddRange(claimsIdentity.Claims.Where(claim => claim.Type == ClaimTypes.Role)
                    .Select(claim => claim.Value));
            }

            var ordersContent = await RequestToEndpoint(_configuration["OrdersApiEndpoint"]);
            var ordersInfo = JArray.Parse(ordersContent).ToObject<List<string>>();

            return new UserProfileViewModel(firstName, lastName, avatarUri, email, roles, ordersInfo);
        }

        private async Task<string> RequestToEndpoint(string requestUri)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}