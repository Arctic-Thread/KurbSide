using KurbSide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KurbSide.ViewComponents
{
    [ViewComponent(Name = "Message")]
    public class MessageViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        public MessageViewComponent(KSContext context) => _context = context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TempData["sysMessage"] = "test";
            return await Task.FromResult((IViewComponentResult)View("Default"));
            //return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
