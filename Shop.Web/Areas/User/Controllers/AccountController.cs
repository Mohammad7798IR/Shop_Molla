using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Wallet;
using Shop.Web.Extentions;
using ZarinpalSandbox;

namespace Shop.Web.Areas.User.Controllers
{
    public partial class AccountController : UserBaseController
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IWalletService walletService, IConfiguration configuration)
        {
            _userService = userService;
            _walletService = walletService;
            _configuration = configuration;
        }
    }

    //Edit Profile Actions

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

    //Wallet Actions


    public partial class AccountController : UserBaseController
    {
        [Route("ChargeWallet")]
        [HttpGet]
        public async Task<IActionResult> ChargeWallet()
        {
            //ToDo : Show List Of Trantions 
            return View();
        }

        [Route("ChargeWallet")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletVM userVM)
        {
            if (ModelState.IsValid)
            {
                var walletId = await _walletService.ChargeWallet(User.GetUserId(), userVM, $"شارژ حساب کاربری به مبلغ {userVM.Amount}");

                var payment = new Payment(userVM.Amount);
                var url = _configuration.GetSection("Host")["DefaultUrl"] + "/UserPanel/payment/" + walletId;
                //  "http://localhost:5235/userpanel/chargewallet"

                var result = payment.PaymentRequest("شارژ کیف پول", url);

                if (result.Result.Status == 100)
                {
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
                }
                else
                {
                    TempData[ErrorMessage] = "مشکلی پیش امد لطفا دوباره تلاش کنید";
                }

            }
            return View();
        }

        [Route("payment/{id}")]
        [HttpGet]
        public async Task<IActionResult> Payment(string id)
        {
            if (HttpContext.Request.Query["status"] != "" && HttpContext.Request.Query["status"].ToString().ToLower() == "ok" && HttpContext.Request.Query["Authority"] != "")
            {
                var authority = HttpContext.Request.Query["Authority"];
                var wallet = await _walletService.GetUserWalletById(id);

                if (wallet != null)
                {
                    var payment = new Payment((int)wallet.Amount);
                    var result = payment.Verification(authority);

                    if (result.Result.Status == 100)
                    {

                        await _walletService.UpdateWalletForCharge(wallet);
                        ViewBag.RefId = result.Result.RefId;
                        ViewBag.IsSuccess = true;
                    }
                    return View();
                }
                return NotFound();
            }
            return View();
        }

    }
}
