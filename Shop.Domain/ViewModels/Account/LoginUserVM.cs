using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class LoginUserVM: Recaptcha
    {

        [Display(Name = " نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام کاربری الزامی می باشد")]
        [DataType(DataType.Text, ErrorMessage = "فرمت نام کاربری وارده صحیح نمی باشد")]
        [RegularExpression(@"\b[^\d\W]+\b", ErrorMessage = "فرمت نام کاربری وارده صحیح نمی باشد . نام کاربری باید تنها از حروف انگلیسی تشکیل شده باشد")]
        public string UserName { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }


        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    public enum LoginResult
    {
        Success,
        NotFound,
        Blocked,
        InActive
    }
}
