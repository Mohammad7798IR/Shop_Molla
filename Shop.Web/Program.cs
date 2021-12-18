using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models.Identity;
using Shop.Infa.Data.Context;
using Shop.Infa.IoC;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;


#region Services

// Add services to the container.

RegisterService(builder.Services);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

builder.Services.AddDbContext<ShopDBContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionStrings"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Cookie.Name = "UserCookies";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(500);
});

#endregion

var app = builder.Build();

#region Middlewares

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(options =>
{
    options.MapControllerRoute
    (
       name: "areas",
       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    options.MapControllerRoute
    (
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});

app.UseStatusCodePagesWithReExecute("/ErrorHandler/{0}");

app.Run();


#endregion



#region IoC

static void RegisterService(IServiceCollection services)
{
    DependencyContainer.RegisterService(services);
}

#endregion
