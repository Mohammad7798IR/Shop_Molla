using Common.ClassHelpers;
using DataFramework.Security;
using Microsoft.AspNetCore.Http;
using SendEmail;
using Shop.Application.ClassHelpers;
using Shop.Application.Extentions;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Application.Senders;
using Shop.Application.Utils;
using Shop.Domain.Interface;
using Shop.Domain.Models.Identity;
using Shop.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.UserServices
{
    public partial class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IViewRenderService _viewRenderService;

        public UserService(IUserRepository userRepository, IViewRenderService viewRenderService)
        {
            _userRepository = userRepository;
            _viewRenderService = viewRenderService;
        }

       
    }

    #region Auth
    public partial class UserService
    {
        public async Task<RegisterResult> RegisterUser(RegisterUserVM userVM)
        {

            if (await _userRepository.UserNameExists(userVM.Username))
            {
                return RegisterResult.UserNameExits;
            }

            if (await _userRepository.EmailExists(userVM.Email))
            {
                return RegisterResult.EmailExits;
            }

            ApplicationUser NewUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Gender = Gender.Unknown,
                Email = userVM.Email,
                HashedPassword = HashGenerator.GenerateHash(userVM.Password),
                IsBlocked = false,
                Avatar = "Default.jpg",
                EmailCode = Guid.NewGuid().ToString(),
                EmailConfirmed = false,
                FirstName = "",
                LastName = "",
                UserName = userVM.Username,
                CreatedAt = PersianDateTime.Now(),
            };
            await _userRepository.AddUser(NewUser);
            await _userRepository.SaveChanges();

            string template = _viewRenderService.RenderToStringAsync("ActiveEmail", NewUser);
            EmailSender.Send(userVM.Email, "فعال سازی", template);

            return RegisterResult.Success;

        }

        public async Task<LoginResult> LoginUser(LoginUserVM userVM)
        {

            var user = await _userRepository.GetByUsername(userVM.UserName);


            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return LoginResult.InActive;
                }

                if (user.IsBlocked)
                {
                    return LoginResult.Blocked;
                }

                if (user.HashedPassword == HashGenerator.GenerateHash(userVM.Password))
                {
                    return LoginResult.Success;
                }
            }

            return LoginResult.NotFound;
        }

        public async Task<ApplicationUser> GetUserByUsername(string userName)
        {
            return await _userRepository.GetByUsername(userName);
        }

        public async Task<ActiveAccountResult> ActiveAccount(string Id)
        {
            var user = await _userRepository.GetByEmailCode(Id);

            if (user != null)
            {
                user.EmailConfirmed = true;
                user.EmailCode = Guid.NewGuid().ToString();
                user.UpdatedAt = PersianDateTime.Now();
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
                return ActiveAccountResult.Success;
            }
            return ActiveAccountResult.NotFound;
        }

        public async Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordUserVM userVM)
        {
            var user = await _userRepository.GetUserByEmail(userVM.Email);
            if (user == null)
            {
                return ForgotPasswordResult.NotFound;
            }
            if (user.EmailConfirmed)
            {
                string template = _viewRenderService.RenderToStringAsync("ForgotPasswordEmail", user);
                EmailSender.Send(userVM.Email, "فراموشی گذرواژه", template);
                return ForgotPasswordResult.Success;
            }
            return ForgotPasswordResult.InActive;
        }

        public async Task<ResetPasswordResult> ResetPassword(ResetPasswordUserVM userVM)
        {
            var user = await _userRepository.GetByEmailCode(userVM.ActiveCode);

            if (user != null)
            {
                user.HashedPassword = HashGenerator.GenerateHash(userVM.Password);
                user.UpdatedAt = PersianDateTime.Now();
                user.EmailCode = Guid.NewGuid().ToString();
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
                return ResetPasswordResult.Success;
            }

            return ResetPasswordResult.Failed;
        }

        public async Task<ApplicationUser> UserExistByEmailCode(string activeCode)
        {
            return await _userRepository.GetByEmailCode(activeCode);
        }
    }

    #endregion


    #region UserPanel

    public partial class UserService
    {
        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<EditUserVM> GetUserEditInformation(string userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserVM()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
            };
        }

        public async Task<EditUserVMResult> EditUser(string userId, IFormFile userAvatar, EditUserVM userVM)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return EditUserVMResult.Failed;
            }

            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.Gender = userVM.Gender;
            user.UpdatedAt = PersianDateTime.Now();

            if (userAvatar != null && CheckContentImage.IsImage(userAvatar))
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(userAvatar.FileName);
                userAvatar.AddImageToServer(imageName, PathImageExtention.UserAvatarOrginServer, 150, 150, PathImageExtention.UserAvatarThumbServer);
                user.Avatar = imageName;
            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();
            return EditUserVMResult.Success;
        }

        public async Task<EditPasswordResult> EditPassword(string userId, EditPasswordVM userVM)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return EditPasswordResult.NotFound;
            }
            if (user.HashedPassword == HashGenerator.GenerateHash(userVM.NewPassword))
            {
                return EditPasswordResult.Equal;
            }
            if (user.HashedPassword == HashGenerator.GenerateHash(userVM.CurrentPassword))
            {
                user.HashedPassword = HashGenerator.GenerateHash(userVM.NewPassword);
                user.UpdatedAt = PersianDateTime.Now();
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
                return EditPasswordResult.Success;
            }
            return EditPasswordResult.WrongCurrentPassword;
        }
    }

    #endregion
}
