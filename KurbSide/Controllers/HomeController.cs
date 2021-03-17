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
using KurbSide.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace KurbSide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, KSContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            
            return accountType switch
            {
                KSCurrentUser.AccountType.BUSINESS => RedirectToAction("Index", "Business"),
                KSCurrentUser.AccountType.MEMBER => RedirectToAction("Index", "Store"),
                _ => RedirectToPage("/Account/Register", new { area = "Identity" })
            };
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

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile formImage)
        {
            ViewData["linkToImage"] = await Service.KSImgur.KSUploadImageToImgur(formImage);
            return View("Sandbox/Sandbox_Image");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
