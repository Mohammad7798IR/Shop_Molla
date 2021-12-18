using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class EditPasswordVM
    {
        [Display(Name ="رمز عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CurrentPassword { get; set; }


        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NewPassword { get; set; }


        [Display(Name = "تکرار رمز عبور ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Compare("NewPassword",ErrorMessage = "کلمه ی عبور با هم مغایرت دارند")]
        public string ConfirmPassword { get; set; }
    }
    public enum EditPasswordResult
    {
        Success,
        NotFound,
        WrongCurrentPassword,
        Equal
    }
}
