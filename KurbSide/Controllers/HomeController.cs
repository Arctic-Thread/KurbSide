using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Geocoding;
using Geocoding.Google;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace KurbSide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, KSContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            if (accountType == "business")
            {
                return RedirectToAction("Index", "Business");
            }
            else if (accountType == "member")
            {
                //TODO Display member "home page", the store maybe? 
                //return RedirectToAction("Index", "Member");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SandboxAsync(string address1 = "108 University Ave Waterloo ON CA N2J 2W2", string address2 = "299 Doon Valley Dr Kitchener ON CA N2G 4M4")
        {

            {
                Service.Location location1 = await Service.GeoCode.GetLocationAsync(address1);
                Service.Location location2 = await Service.GeoCode.GetLocationAsync(address2);

                //var distance = await Service.GeoCode.CalculateDistanceAsync(location1, location2);
                var distance = Service.GeoCode.CalculateDistanceLocal(location1, location2);

                TempData["ln1"] = $"{location1.address}, {location2.address}";
                TempData["ln2"] = $"{distance.distance}m, {distance.time} sec";
                TempData["ln3"] = $"{address1}";
                TempData["ln4"] = $"{address2}";

            }

            return View("Sandbox/Sandbox");
        }

        /// <summary>
        /// Uploads an image in a form to the Imgur via their API
        /// </summary>
        /// <remarks>
        /// TODO It has literally zero validation or error checking.
        /// You can add any file type, any size, viruses, etc.
        /// <br/> Liam De Rivers
        /// </remarks>
        /// <param name="formImage">The image to be uploaded from the form</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImageToImgur(IFormFile formImage)
        {
            using (var client = new HttpClient())
            {
                string imgurApiUrl = "https://api.imgur.com/3/";
                string imgurSecret = Environment.GetEnvironmentVariable("imgur_api_secret");
                string imgurClientID = "Client-ID 1f6dd4de0fc6655";

                client.BaseAddress = new Uri(imgurApiUrl);
                client.DefaultRequestHeaders.Add("Authorization", imgurClientID);
                client.DefaultRequestHeaders.Add("Token", imgurSecret);

                try
                {
                    MultipartFormDataContent formData = new MultipartFormDataContent(); // The form to be uploaded to the Imgur API

                    using (var memoryStream = new MemoryStream())
                    {
                        await formImage.CopyToAsync(memoryStream); // Saves the entire image file (creation date, author etc.) to memory
                        var rawContents = memoryStream.ToArray(); // rawContents is the image contents alone
                        HttpContent imageContents = new StringContent(Convert.ToBase64String(rawContents)); // imageContents is an httpContent entity containing the image contents
                        formData.Add(imageContents, "image"); // Add the imageContents to the "form"

                        HttpResponseMessage rawResponse = await client.PostAsync("image", formData); // The raw response from the Imgur API
                        var responseContent = await rawResponse.Content.ReadAsStringAsync(); // The response content from the Imgur API
                        var jsonResponseContent = JsonConvert.DeserializeObject<dynamic>(responseContent); // "Json-ified" response content
                        ViewData["linkToImage"] = jsonResponseContent["data"]["link"]; // The link to the image
                        return View("Sandbox/Sandbox_Image");
                    }
                }
                catch (Exception) { throw; }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region CurrentUserUtils
        //Current User Utils 1.0
        public Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<string> GetLoggedInEmailAsync()
        {
            var user = await GetCurrentUserAsync();
            return user == null ? "" : user.Email;
        }

        public string GetAccountType(IdentityUser? IUser)
        {
            if (IUser == null) return "";

            bool hasBusiness = _context.Business.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;
            bool hasMember = _context.Member.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;

            /*
             * HACK maybe make this an enum rather than a string to prevent future issues
             *      actually nevermind because C# enums aren't like Java enums
             *      for now keeping it standard to lowercase strings works :)
             */
            if (hasBusiness) return "business";
            else if (hasMember) return "member";
            else return "";
        }
        //End Current User Utils 1.0
        #endregion
    }
}
