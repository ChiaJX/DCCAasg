using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sportswear.Areas.Identity.Data;
using Sportswear.Data;

[assembly: HostingStartup(typeof(Sportswear.Areas.Identity.IdentityHostingStartup))]
namespace Sportswear.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SportswearContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SportswearContextConnection")));

                services.AddDefaultIdentity<SportswearUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SportswearContext>();
            });
        }
    }
}