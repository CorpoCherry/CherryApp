using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cherry.Web
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
            // ================ DB Contexts ================
            services.AddDbContext<IdentityDb>
            (options => options.UseMySQL(Configuration.GetConnectionString("MySQL_Identity" + Parameters.DBtype)));
            services.AddDbContext<ConfigurationContextDb>
            (options => options.UseMySQL(Configuration.GetConnectionString("MySQL_Configuration" + Parameters.DBtype)));
            services.AddDbContext<SchoolDb>();
            services.AddScoped<IConfigurationContextDb>(provider => provider.GetService<ConfigurationContextDb>());

            // ================ Identity ================
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDb>()
            .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // ================ MVC ================
            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                option.Filters.Add(new AuthorizeFilter(policy));

            });

            // ================ Cookies ================
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = new TimeSpan(0, 0, 0, 0, 1);
                options.LoginPath = "/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IdentityDb identityDb, UserManager<User> userManager, ConfigurationContextDb app_configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            // ================ Identity ================
            if (identityDb.Database.EnsureCreated())
            {                
                var root = new User
                {
                    FirstName = "Alan (ROOT)",
                    LastName = "Borowy",
                    UserName = "root",
                    Email = "root@cherryapp.pl",
                    EmailConfirmed = true
                };

                var manager = new User
                {
                    FirstName = "Alan (MGR)",
                    LastName = "Borowy",
                    UserName = "manager",
                    Email = "manager@cherryapp.pl",
                    EmailConfirmed = true
                };

                var user = new User
                {
                    FirstName = "Alan (USER)",
                    LastName = "Borowy",
                    UserName = "user",
                    Email = "user@cherryapp.pl",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(root, "rootBasic98");
                userManager.CreateAsync(manager, "managerBasic98");
                userManager.CreateAsync(user, "userBasic98");
            }

            // ================ Config ================
            if (app_configuration.Database.EnsureCreated())
            {
                app_configuration.Add(new School
                {
                    OfficialName = "Zespół Szkół Podstawowych nr 2",
                    PseudoName = "ZSP2",
                    NamedBy = "Kornel Makuszyński",
                    City = "Legionowo",
                    Country = "PL",
                    Adrress = "Jagiellońska 67",
                    Tag = "zsp2legionowo"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Szkoła Podstawowa nr 314",
                    PseudoName = "SP314",
                    NamedBy = "Przyjaciół Ziemi",
                    City = "Warszawa",
                    Country = "PL",
                    Adrress = "Porajów 3",
                    Tag = "sp314warszawa"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Zespół Szkół Samochodowych",
                    PseudoName = "ZSS",
                    NamedBy = "Sportowców Ziemii Szczecińskiej",
                    City = "Szczecin",
                    Country = "PL",
                    Adrress = "Małopolska 22",
                    Tag = "zssszczecin"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Zespół Szkół Specjalnych nr 2",
                    PseudoName = "ZSS2",
                    NamedBy = null,
                    City = "Gdańsk",
                    Country = "PL",
                    Adrress = "Witastwosza 23",
                    Tag = "zss2gdańsk"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Sopocka Szkoła Montessori",
                    PseudoName = "SSP",
                    NamedBy = null,
                    City = "Sopot",
                    Country = "PL",
                    Adrress = "Tatrzańska 19",
                    Tag = "sspsopot"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Szkoła Podstawowa nr 84",
                    PseudoName = "SP",
                    NamedBy = "Ruch Oporu Pokoju",
                    City = "wrocław",
                    Country = "PL",
                    Adrress = "Górnickiego 20",
                    Tag = "spwroclaw"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "XII Liceum Ogólnokształcące",
                    PseudoName = "XIILO",
                    NamedBy = "M. Skłodwska-Curie",
                    City = "Poznań",
                    Country = "PL",
                    Adrress = "Kutrzeby 8",
                    Tag = "XIIlieceumogolnoksztalcace"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Wyższa Szkoła Techniczna",
                    PseudoName = "WST",
                    NamedBy = null,
                    City = "Katowice",
                    Country = "PL",
                    Adrress = "Rolna 43",
                    Tag = "wyzszaszkolatechniczna"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Śląski Uniwersytet Medyczny",
                    PseudoName = "ŚUM",
                    NamedBy = null,
                    City = "Katowice",
                    Country = "PL",
                    Adrress = "Poniatowskiego 15",
                    Tag = "slaskiuniwerekmedyczny"
                });
                app_configuration.Add(new School
                {
                    OfficialName = "Szkoła Podstawowa nr 4",
                    PseudoName = "SP",
                    NamedBy = "T. Kościuszki. Filla",
                    City = "Katowice",
                    Country = "PL",
                    Adrress = "Józefowska 52/54",
                    Tag = "szkolapodtawowakatowice"
                });
                app_configuration.SaveChanges();

                //
            }

            // ================ Schools ================
            //if(!schoolsDb.Database.EnsureCreated())
            //{
            //    //ONLY FOR TEST
            //    schoolsDb.Database.EnsureDeleted();
            //    schoolsDb.Database.EnsureCreated();
            //    //
            //}
        }
    }
}
