using Microsoft.AspNetCore.Mvc;
using Shop.Application.Extentions;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Web.Extentions;

namespace Shop.Web.Areas.ViewComponents
{
    public class UserSideBarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public UserSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                return View("UserSideBar", user);
            }
            return View("UserSideBar");
        }
    }

    public class UserInformationViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public UserInformationViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                return View("UserInformation",user);
            }
            return View("UserInformation");
        }
    }
}
