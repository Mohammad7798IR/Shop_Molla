using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Identity
{
    public class BaseEntity
    {
        [Display(Name = "تاریخ ساخت")]
        public string? CreatedAt { get; set; }

        [Display(Name = "تاریخ تغییر")]
        public string? UpdatedAt { get; set; }
    }
}
