using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interface;
using Shop.Domain.Models.Wallet;
using Shop.Infa.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infa.Data.Repositories
{
    public partial class WalletRepository : IWalletRepository
    {
        private readonly ShopDBContext _context;

        public WalletRepository(ShopDBContext context)
        {
            _context = context;
        }

       
    }
    public partial class WalletRepository
    {
        public async Task CreateWallet(UserWallet wallet)
        {
            await _context.UserWallet.AddAsync(wallet);
        }

        public async Task SaveChanges()
        {
           await _context.SaveChangesAsync();
        }

        public void UpdateChanges(UserWallet wallet)
        {
            _context.UserWallet.Update(wallet);
        }

        public async Task<UserWallet> GetUserWalletById(string id)
        {
            return await _context.UserWallet.AsQueryable().SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
