using KurbSide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KurbSide.ViewComponents
{
    [ViewComponent(Name = "AutoRefresh")]
    public class AutoRefreshViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        public AutoRefreshViewComponent(KSContext context) => _context = context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
