using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Shop.Application.Interfaces.UserInterfaces;
using Shop.Application.Services.UserServices;
using Shop.Domain.Interface;
using Shop.Infa.Data.Repositories;
using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using SendEmail;

namespace Shop.Infa.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {

            #region Services

            services.AddScoped<IUserService, UserService>();

            #endregion


            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            #endregion


            #region Tools

            services.AddSingleton<HtmlEncoder>
                (HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));


            services.AddScoped<IViewRenderService, RenderViewToString>();

            #endregion

        }
    }
}
