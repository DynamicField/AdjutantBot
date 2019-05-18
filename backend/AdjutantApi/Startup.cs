using System;
using System.Security.Claims;
using AdjutantApi.Data;
using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace AdjutantApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentity<AdjutantUser, IdentityRole>()
                .AddEntityFrameworkStores<AdjutantContext>()
                .AddDefaultTokenProviders();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddDiscord(discordOptions =>
                {
                    discordOptions.ClientId = Configuration["Authentication:Discord:AppId"];
                    discordOptions.ClientSecret = Configuration["Authentication:Discord:AppSecret"];
                    discordOptions.Scope.Add("guilds identify");
                    discordOptions.SaveTokens = true;
                    discordOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "discriminator");
                    discordOptions.ClaimActions.MapJsonKey(ClaimTypes.UserData, "avatar");
                });
            
            services.AddDbContext<AdjutantContext>
                (options =>
            {
                options.UseNpgsql(Configuration["Database:PostgreSQL:ConnectionString"], b => { b.MigrationsAssembly("AdjutantApi"); });
            });

            services.AddSingleton<IConfiguration>(Configuration.GetSection("Adjutant"));
            
            services.AddCors(o => o.AddPolicy("AllowAny", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

        }

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

            app.UseCors("AllowAny");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}