using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
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

        /// <summary>
        /// Displays the home page for different account types.
        /// Business -> Business Dashboard.
        /// Member -> Store front.
        /// Visitor -> Registration page.
        /// </summary>
        /// <returns>A redirect to the "home page" for each account type.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);
            
            return accountType switch
            {
                KSUserUtilities.AccountType.BUSINESS => RedirectToAction("Index", "Business"),
                KSUserUtilities.AccountType.MEMBER => RedirectToAction("Index", "Store"),
                _ => RedirectToPage("/Account/Register", new { area = "Identity" })
            };
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>A redirect to the privacy policy page.</returns>
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
