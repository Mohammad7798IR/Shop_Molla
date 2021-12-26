using Common.ClassHelpers;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Domain.Interface;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.UserServices
{
    public partial class WalletService : IWalletService
    {
        private readonly IUserService _userService;
        private readonly IWalletRepository _walletRepository;
        public WalletService(IUserService userService, IWalletRepository walletRepository)
        {
            _userService = userService;
            _walletRepository = walletRepository;
        }


    }
    public partial class WalletService
    {
        public async Task<string> ChargeWallet(string userId, ChargeWalletVM userVM, string description)
        {
            var wallet = new UserWallet
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Amount = userVM.Amount,
                Description = description,
                WalletType = WalletType.variz,
                IsPaid = false,
                CreatedAt = PersianDateTime.Now()
            };

            await _walletRepository.CreateWallet(wallet);
            await _walletRepository.SaveChanges();

            return wallet.Id;

        }

        public async Task<bool> UpdateWalletForCharge(UserWallet userWallet)
        {
            if (userWallet != null)
            {
                userWallet.IsPaid = true;
                userWallet.WalletType = WalletType.variz;
                _walletRepository.UpdateChanges(userWallet);
                await _walletRepository.SaveChanges();
                return true;
            }
            return false;

        }


        public async Task<UserWallet> GetUserWalletById(string id)
        {
            return await _walletRepository.GetUserWalletById(id);
        }
    }
}
