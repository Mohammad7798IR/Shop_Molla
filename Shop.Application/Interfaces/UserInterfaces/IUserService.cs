using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Identity;
using Shop.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<RegisterResult> RegisterUser(RegisterUserVM userVM);

        Task<LoginResult> LoginUser(LoginUserVM userVM);

        Task<ActiveAccountResult> ActiveAccount(string Id);

        Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordUserVM userVM);

        Task<ResetPasswordResult> ResetPassword(ResetPasswordUserVM userVM);

        Task<ApplicationUser> UserExistByEmailCode(string activeCdoe);

        Task<ApplicationUser> GetUserByUsername(string Username);

        Task<ApplicationUser> GetUserById(string id);


        Task<EditUserVM> GetUserEditInformation(string userId);

        Task<EditUserVMResult> EditUser(string userId, IFormFile userAvatar, EditUserVM userVM);


        Task<EditPasswordResult> EditPassword(string userId, EditPasswordVM userVM);
    }
}
