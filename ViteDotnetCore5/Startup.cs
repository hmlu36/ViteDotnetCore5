
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
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
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            
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

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/build";
            });
            
            // 使用memory cache
            services.AddMemoryCache();

            InjectService(services);
        }

        // DI(Dependency Injection) 注入service
        private void InjectService(IServiceCollection services) {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // configure DI for application services
            // 帳號、角色、公司別相關資訊
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) {
                    spa.UseViteDevelopmentServer();
                }
            });

            ExtendDownloadFile(app);
        }

        // 其他未定義MIME檔名下載
        private void ExtendDownloadFile(IApplicationBuilder app) {
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".yaml", "application/octet-stream");

            app.UseStaticFiles(new StaticFileOptions {
                ContentTypeProvider = provider
            });
        }
    }
}
