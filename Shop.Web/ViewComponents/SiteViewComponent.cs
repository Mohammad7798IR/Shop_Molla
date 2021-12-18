using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.UserInterfaces;

namespace Shop.Web.ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IUserService _service;

        public SiteHeaderViewComponent(IUserService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _service.GetUserByUsername(User.Identity.Name);
            }
        
            return View("SiteHeader");
        }
    }
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
    }
}
