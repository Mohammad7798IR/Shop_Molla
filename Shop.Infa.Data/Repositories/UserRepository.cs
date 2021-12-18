using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interface;
using Shop.Domain.Models.Identity;
using Shop.Infa.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infa.Data.Repositories
{
    public partial class UserRepository : IUserRepository
    {
        private readonly ShopDBContext _context;

        public UserRepository(ShopDBContext context)
        {
            _context = context;
        }

     
    }
    public partial class UserRepository
    {

        public async Task<bool> EmailExists(string email)
        {
            return await _context.ApplicationUser.AsQueryable().AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserNameExists(string UserName)
        {
            return await _context.ApplicationUser.AsQueryable().AnyAsync(u => u.UserName == UserName);
        }

        public async Task<ApplicationUser> GetByEmailCode(string code)
        {
            return await _context.ApplicationUser.AsQueryable().FirstOrDefaultAsync(u => u.EmailCode == code);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _context.ApplicationUser.AsQueryable().SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetByUsername(string UserName)
        {
            return await _context.ApplicationUser.AsQueryable().SingleOrDefaultAsync(u => u.UserName == UserName);
        }


        public async Task AddUser(ApplicationUser user)
        {
            await _context.ApplicationUser.AddAsync(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _context.ApplicationUser.Update(user);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _context.ApplicationUser.AsQueryable().SingleOrDefaultAsync(c => c.Id == id);
        }
    }

    #region SaveChanges

    public partial class UserRepository
    {
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

    #endregion
}
