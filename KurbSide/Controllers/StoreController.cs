using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Business> _logger;

        public StoreController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Business> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
