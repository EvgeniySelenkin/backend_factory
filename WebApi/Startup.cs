using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Authorization;
using WebApi.BackgroundServices;

namespace WebApi
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
            services.AddDbContext<DBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DataConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // ????????, ????? ?? ?????????????? ???????? ??? ????????? ??????
                            ValidateIssuer = true,
                            // ??????, ?????????????? ????????
                            ValidIssuer = AuthOptions.ISSUER,

                            // ????? ?? ?????????????? ??????????? ??????
                            ValidateAudience = true,
                            // ????????? ??????????? ??????
                            ValidAudience = AuthOptions.AUDIENCE,
                            // ????? ?? ?????????????? ????? ?????????????
                            ValidateLifetime = true,

                            // ????????? ????? ????????????
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // ????????? ????? ????????????
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddScoped<FactoryRepository>();
            services.AddScoped<UnitRepository>();
            services.AddScoped<TankRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddHostedService<BackgroundService>();
            services.AddHostedService<EventBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
