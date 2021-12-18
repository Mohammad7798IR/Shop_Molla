using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Domain.ViewModels.Account;
using Shop.Web.Extentions;

namespace Shop.Web.Areas.User.Controllers
{
    public partial class AccountController : UserBaseController
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;

        }
    }



    public partial class AccountController : UserBaseController
    {
        [HttpGet]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var result = await _userService.GetUserEditInformation(User.GetUserId());

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        [Route("EditProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserVM userVM, IFormFile userAvatar)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUser(User.GetUserId(), userAvatar, userVM);
                switch (result)
                {
                    case EditUserVMResult.Success:
                        TempData[SuccessMessage] = "ویرایش با موفقیت انجام شد";
                        return View();
                    case EditUserVMResult.Failed:
                        TempData[ErrorMessage] = "مشکلی پیش امد لطفا دوباره تلاش کنید";
                        return View();
                    default:
                        break;
                }
            }
            return View();
        }


        [HttpGet]
        [Route("EdotPassword")]
        public async Task<IActionResult> EditPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("EdotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordVM userVM)
        {
            var result = await _userService.EditPassword(User.GetUserId(), userVM);
            switch (result)
            {
                case EditPasswordResult.Success:
                    TempData[SuccessMessage] = "ویرایش با موفقیت انجام شد";
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "PublicAuth", new { area = "" });
                case EditPasswordResult.NotFound:
                    TempData[ErrorMessage] = "مشکلی پیش امد لطفا دوباره تلاش کنید";
                    return View();
                case EditPasswordResult.Equal:
                    TempData[InfoMessage] = "لطفا از کلمه عبور جدیدی استفاده کنید";
                    ModelState.AddModelError("NewPassword", "لطفا از کلمه عبور جدیدی استفاده کنید");
                    return View();
                case EditPasswordResult.WrongCurrentPassword:
                    TempData[ErrorMessage] = "رمز عبور وارد شده صحیح نمی باشد";
                    return View();
                default:
                    break;
            }
            return View(result);
        }

    }
}
