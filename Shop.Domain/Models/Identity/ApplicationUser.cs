using Shop.Domain.Models.Wallet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shop.Domain.Models.Identity
{
    public partial class ApplicationUser : BaseEntity
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "پست الکترونیک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string HashedPassword { get; set; }

        [Display(Name = "آواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Avatar { get; set; }

        [Display(Name = "مسدود شده / نشده")]
        public bool IsBlocked { get; set; }

        [Display(Name = "کد احرازهویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string EmailCode { get; set; }

        [Display(Name = "تایید شده / نشده")]
        public bool EmailConfirmed { get; set; }


        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        #region UserDetails


        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }


        [Display(Name = "تاریخ تولد")]
        public string? BirthDate { get; set; }


        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }

        #endregion

    }

    public partial class ApplicationUser
    {
        public ICollection<UserWallet> UserWallets { get; set; }
    }

    public enum Gender
    {
        [Display(Name ="اقا")]
        Male,
        [Display(Name = "خانوم")]
        Female,
        [Display(Name = "نامشخص")]
        Unknown
    }
}
