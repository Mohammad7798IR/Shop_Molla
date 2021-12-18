using Shop.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interface
{
    public partial interface IUserRepository
    {

        Task<bool> EmailExists(string email);

        Task<bool> UserNameExists(string UserName);

        Task<ApplicationUser> GetUserByEmail(string email);

        Task<ApplicationUser> GetByEmailCode(string code);

        Task<ApplicationUser> GetByUsername(string UserName);

        Task AddUser(ApplicationUser user);

        void UpdateUser(ApplicationUser user);

        Task SaveChanges();

        Task<ApplicationUser> GetUserById(string id);
    }
}
