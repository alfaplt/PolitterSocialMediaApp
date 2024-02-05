using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Mapping;
using Business.Validations.CustomValidations;
using Business.Validations.UserValidations;
using Core.Entities;
using DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvc.Hubs;
using System;

namespace Mvc
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation(); 
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            
            services.AddValidatorsFromAssemblyContaining<AppUserRegisterDtoValidator>(); //For all validators
            services.AddFluentValidationAutoValidation();         

            services.AddAutoMapper(typeof(MapProfile));
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

			#region Dependency Injections
			services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFollowService, FollowService>();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
			#endregion

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            services.AddIdentity<AppUser, AppRole>(_ =>
            {
                _.Password.RequiredLength = 5;
                _.Password.RequireNonAlphanumeric = false;
                _.Password.RequireLowercase = false;
                _.Password.RequireUppercase = false;
                _.Password.RequireDigit = false;
                _.User.RequireUniqueEmail = true;
            })
                .AddPasswordValidator<CustomPasswordValidation>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(_ =>
            {
                _.LoginPath = new PathString("/Account/SignIn");
                _.Cookie = new CookieBuilder
                {
                    Name = "AspNetCoreIdentityExampleCookie", //Oluþturulacak Cookie'yi isimlendiriyoruz.
                    HttpOnly = false, //Kötü niyetli insanlarýn client-side tarafýndan Cookie'ye eriþmesini engelliyoruz.
                    //MaxAge = TimeSpan.FromMinutes(30),
                    SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin gönderilmemesini belirtiyoruz.
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //Cookie’leri hem HTTP hemde HTTPS protokolü üzerinden eriþilebilir yapar.
                };
                _.SlidingExpiration = true; //Expiration süresinin yarýsý kadar süre zarfýnda istekte bulunulursa eðer geri kalan yarýsýný tekrar sýfýrlayarak ilk ayarlanan süreyi tazeleyecektir.
                _.ExpireTimeSpan = TimeSpan.FromHours(1); //CookieBuilder nesnesinde tanýmlanan Expiration deðerinin varsayýlan deðerlerle ezilme ihtimaline karþýn tekrardan Cookie vadesi burada da belirtiliyor.
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
