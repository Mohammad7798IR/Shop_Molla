using Microsoft.AspNetCore.Mvc;

namespace Shop.Web.Controllers
{
   
    public class ErrorHandlerController : Controller
    {
        [Route("/ErrorHandler/{code}")]
        public IActionResult Index(int code)
        {
            switch (code)
            {
                case 404:
                    return View("NotFound");
                default:
                    break;
            }
            return View();
        }
    }
}
