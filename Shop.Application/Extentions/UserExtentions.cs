using Shop.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Extentions
{
    public static class UserExtentionsuser
    {
        public static string GetUser(this ApplicationUser User)
        {

            if (!string.IsNullOrEmpty(User.FirstName) && !string.IsNullOrEmpty(User.LastName))
            {
                return $"{User.FirstName}/{User.LastName} ";
            }
            return User.UserName;
        }
    }
}
