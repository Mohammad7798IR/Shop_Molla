using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class RegisterUserVM : Recaptcha
    {

        //[Display(Name = "نام")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //public string FirstName { get; set; }

        //[Display(Name = "نام خانوادگی")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //public string LastName { get; set; }

        [Display(Name = " نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام کاربری الزامی می باشد")]
        [DataType(DataType.Text, ErrorMessage = "فرمت نام کاربری وارده صحیح نمی باشد")]
        [RegularExpression(@"\b[^\d\W]+\b", ErrorMessage = "فرمت نام کاربری وارده صحیح نمی باشد . نام کاربری باید تنها از حروف انگلیسی تشکیل شده باشد")]
        public string Username { get; set; }

        [Display(Name = "پست الکترونیک")]
        [EmailAddress(ErrorMessage = "فرمت پست الکترونیک صحیح نمی باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{12,}"
        //    , ErrorMessage = "رمز عبور شما باید یک رمز عبور قوی ( شامل حداقل یک حرف بزرگ و یک حرف کوچک ؛ عدد ؛ 12 کاراکتری ) باشد")]
        public string Password { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("Password", ErrorMessage = "کلمه ی عبور با هم مغایرت دارند")]
        public string ConfirmPassword { get; set; }
    }
    public enum RegisterResult
    {
        Success,
        EmailExits,
        UserNameExits
    }
}
