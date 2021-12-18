using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models.Identity;
using Shop.Domain.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infa.Data.Context
{
    public partial class ShopDBContext : DbContext
    {
        public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<UserWallet> UserWallet { get; set; }
    }

    public partial class ShopDBContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<UserWallet>().ToTable("Wallet");


            builder.Entity<UserWallet>().HasKey(pk => pk.Id);


            builder.Entity<UserWallet>()
                .HasOne(AU => AU.ApplicationUser)
                .WithMany(UW=>UW.UserWallets)
                .HasForeignKey(fk=>fk.UserId);




            #region ApplicationUser


            //builder.Entity<ApplicationUser>()
            //    .HasKey(pk => pk.Id);


            #endregion


            #region ApplicationRole

            //builder.Entity<ApplicationRole>()
            //    .HasKey(pk=>pk.Id);

            #endregion



        }
    }
}
