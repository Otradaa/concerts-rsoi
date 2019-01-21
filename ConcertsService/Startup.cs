using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ConcertsService.Models;
using ConcertsService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ConcertsService.Auth;
using Microsoft.AspNetCore.Identity;

namespace ConcertsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ConcertsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ConcertsContext")));

            services.AddScoped<IConcertRepository, ConcertRepository>();

            //services.AddIdentityCore<IdentityUser>(); 

            var accountOptions = new TokenParameters()

            {
                ISSUER = "authServer",
                AUDIENCE = "GateWay"
            };
            var tokenGenerator = new Token(accountOptions);

            services.AddSingleton<IToken>(tokenGenerator);

            services.AddSingleton<TokenParameters>(accountOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = accountOptions.GetParameters();

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
