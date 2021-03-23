using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ViteDotnetCore5.Extensions;
using ViteDotnetCore5.Models.Config;
using ViteDotnetCore5.Models.EFCore;
using ViteDotnetCore5.Services;
using ViteDotnetCore5.Utils;

namespace ViteDotnetCore5 {
    public class Startup {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) {
            // 將設定資料存取成singleton
            configuration.GetSection("JwtSettings").Bind(JwtSettings.Instance);
            configuration.GetSection("GoogleRecaptcha").Bind(GoogleRecaptcha.Instance);

            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi", Version = "v1" });
            });
            services.AddDbContext<GeoEPPContext>(options => options.UseSqlServer(_configuration.GetConnectionString("GeoEPP"))
                                                                    // 啟用 Logging 觀察 SQL 指令
                                                                    //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                                                                    // 顯示SQL參數
                                                                    //.EnableSensitiveDataLogging()
                                                                    );

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(JwtSettings.Instance.Secret);
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "wwwroot";
            });

            InjectService(services);
        }

        // DI(Dependency Injection) 注入service
        private void InjectService(IServiceCollection services) {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRecaptchaExtension, RecaptchaExtension>();
            services.AddHttpClient();

            // configure DI for application services
            // 帳號、角色、公司別相關資訊
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi v1"));
            } else {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();

                if (!env.IsDevelopment()) {
                    endpoints.MapFallbackToFile("/index.html"); // 預設導回index.html
                }
            });

            if (env.IsDevelopment()) {
                app.UseSpa(spa => {
                    spa.Options.SourcePath = "wwwroot/vite";
                    spa.UseViteDevelopmentServer();
                });
            }

        }
    }
}
