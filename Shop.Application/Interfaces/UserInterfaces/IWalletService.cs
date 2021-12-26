using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces.UserInterfaces
{
    public interface IWalletService
    {
        Task<string> ChargeWallet(string userId, ChargeWalletVM userVM, string description);


        Task<bool> UpdateWalletForCharge(UserWallet userWallet);

        Task<UserWallet> GetUserWalletById(string id);
    }
}
