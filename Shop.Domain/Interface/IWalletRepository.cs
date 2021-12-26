using Shop.Domain.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interface
{
    public partial interface IWalletRepository
    {
        Task CreateWallet(UserWallet wallet);


        Task<UserWallet> GetUserWalletById(string id);

        Task SaveChanges();

        void UpdateChanges(UserWallet wallet);
    }
}
