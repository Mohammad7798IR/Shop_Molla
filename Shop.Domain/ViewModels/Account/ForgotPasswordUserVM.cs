using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class ForgotPasswordUserVM : Recaptcha
    {
        [Display(Name = "پست الکترونیک")]
        [EmailAddress(ErrorMessage = "فرمت پست الکترونیک صحیح نمی باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }
    }
    public enum ForgotPasswordResult
    {
        Success,
        InActive,
        NotFound
    }
}
