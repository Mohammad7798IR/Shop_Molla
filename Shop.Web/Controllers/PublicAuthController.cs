using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Domain.Models.Identity;
using Shop.Domain.ViewModels.Account;
using System.Security.Claims;

namespace Shop.Web.Controllers
{
    public partial class PublicAuthController : BaseController
    {
        private readonly IUserService _service;
        private readonly ICaptchaValidator _validator;

        public PublicAuthController(IUserService service, ICaptchaValidator validator)
        {
            _service = service;
            _validator = validator;
        }

    }
    public partial class PublicAuthController
    {
        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM userVM)
        {
            if (ModelState.IsValid)
            {

                if (!await _validator.IsCaptchaPassedAsync(userVM.Token))
                {
                    TempData[ErrorMessage] = "اعتبار سنجی ReCaptcha انجام نشد";
                    return View(userVM);
                }

                var result = await _service.RegisterUser(userVM);
                switch (result)
                {
                    case RegisterResult.Success:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد ";
                        TempData[InfoMessage] = "پست الکترونیک فعال سازی برای شما ارسال شد";
                        ViewBag.IsSuccessfully = true;
                        return View();

                    case RegisterResult.UserNameExits:
                        TempData[ErrorMessage] = "نام کاربری قبلا انتخاب شده است";
                        ViewBag.IsSuccessfully = false;
                        return View();

                    case RegisterResult.EmailExits:
                        TempData[ErrorMessage] = "پست الکترونیک قبلا ثبت شده است";
                        ViewBag.IsSuccessfully = false;
                        return View();

                    default:
                        break;
                }
            }

            return View(userVM);
        }

        #endregion


        #region ActiveAccount

        public async Task<IActionResult> ActiveAccount(string Id)
        {
            var result = await _service.ActiveAccount(Id);
            switch (result)
            {
                case ActiveAccountResult.Success:
                    TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                    return RedirectToAction("Login");
                case ActiveAccountResult.NotFound:
                    return NotFound();
                default:
                    break;
            }
            return View();
        }


        #endregion


        #region login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (!await _validator.IsCaptchaPassedAsync(userVM.Token))
                {
                    TempData[ErrorMessage] = "اعتبار سنجی ReCaptcha انجام نشد";
                    return View(userVM);
                }
                var result = await _service.LoginUser(userVM);
                switch (result)
                {

                    case LoginResult.Success:

                        var user = await _service.GetUserByUsername(userVM.UserName);

                        var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name , user.UserName),
                            new Claim(ClaimTypes.NameIdentifier , user.Id.ToString())
                        };

                        var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

                        var claimPrincipal = new ClaimsPrincipal(identity);

                        var cookieProperties = new AuthenticationProperties
                        {
                            IsPersistent = userVM.RememberMe
                        };
                        await HttpContext.SignInAsync(claimPrincipal, cookieProperties);

                        TempData[SuccessMessage]="ورود شما با موفقیت انجام شد";
                        return Redirect("/");

                    case LoginResult.NotFound:
                        TempData[ErrorMessage] = "حساب کاربری پیدا نشد";
                        ViewBag.IsSuccessfully = false;
                        return View(userVM);

                    case LoginResult.Blocked:
                        TempData[ErrorMessage] = "حساب کاربری شما مسدود شده است";
                        return Redirect("/");

                    case LoginResult.InActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        ViewBag.IsSuccessfully = false;
                        return View();

                    default:
                        break;
                }
            }
            return View();
        }
        #endregion


        #region LogOut

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion


        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (!await _validator.IsCaptchaPassedAsync(userVM.Token))
                {

                    TempData[ErrorMessage] = "اعتبار سنجی ReCaptcha انجام نشد";
                    return View(userVM);

                }
                var result = await _service.ForgotPassword(userVM);
                ViewBag.IsSuccessfully = true;
                switch (result)
                {
                    case ForgotPasswordResult.Success:
                        TempData[InfoMessage] = "پست الکترونیک فعال سازی برای شما ارسال شد";
                        return Redirect("/");
                    case ForgotPasswordResult.InActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        ViewBag.IsSuccessfully = false;
                        return View();
                    case ForgotPasswordResult.NotFound:
                        TempData[ErrorMessage] = "پست الکترونیک وارد شده پیدا نشد";
                        ViewBag.IsSuccessfully = false;
                        return View(userVM);
                    default:
                        break;
                }
            }
            return View();
        }

        #endregion


        #region ResetPassword


        public async Task<IActionResult> ResetPassword(string id)
        {
            var result = await _service.UserExistByEmailCode(id);

            if (result == null)
                return NotFound();

            return View(new ResetPasswordUserVM
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                if (!await _validator.IsCaptchaPassedAsync(userVM.Token))
                {
                    TempData[ErrorMessage] = "اعتبار سنجی ReCaptcha انجام نشد";
                    return View(userVM);
                }

                var result = await _service.ResetPassword(userVM);
                switch (result)
                {
                    case ResetPasswordResult.Success:
                        TempData[SuccessMessage] = "رمز عبور شما با موفقیت تغییر کرد";
                        return RedirectToAction("login");

                    case ResetPasswordResult.Failed:
                        TempData[ErrorMessage] = "مشکلی پیش امد لطفا دوباره تلاش کنید";
                        ViewBag.IsSuccessfully = false;
                        return View();

                    default:
                        break;
                }
            }
            return View();
        }

        #endregion


    }
}
