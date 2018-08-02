using System;
using Cherry.Data.Configuration;
using Cherry.Data.Identity;
using Cherry.Data.School;
using Cherry.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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

        private readonly IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ================ DB Contexts ================
            services.AddDbContext<ConfigurationContext>();
            services.AddSingleton<IConfigurationContext, ConfigurationContext>();
            //services.AddScoped<IConfigurationContext>(provider => provider.GetService<ConfigurationContext>());


            services.AddDbContext<IdentityContext>();
            services.AddDbContext<SchoolContext>();           

            // ================ Identity ================
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();
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

            // ================ Other Services ================

            services.AddScoped<IUserProfileLoader, UserServices>();

            // ================ MVC ================
            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                option.Filters.Add(new AuthorizeFilter(policy));

            });

            // ================ Cookies ================

            services.ConfigureApplicationCookie(options => {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = new TimeSpan(0, 0, 0, 1);
                options.SlidingExpiration = true;

                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/AccessDenied";
            });

            // ================ GDPR/RODO ================
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebpackDevMiddleware();
                // HMR (not working, webpack.config.js need fix)
                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true
                //});
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app_configuration.Add(new School
            //{
            //    OfficialName = "Zespół Szkół Podstawowych nr 2",
            //    PseudoName = "ZSP2",
            //    NamedBy = "Kornel Makuszyński",
            //    City = new City { Name = "Legionowo" },
            //    Country = "PL",
            //    Adrress = "Jagiellońska 67",
            //    Tag = "zsp2legionowo"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Szkoła Podstawowa nr 314",
            //    PseudoName = "SP314",
            //    NamedBy = "Przyjaciół Ziemi",
            //    City = new City { Name = "Warszawa" },
            //    Country = "PL",
            //    Adrress = "Porajów 3",
            //    Tag = "sp314warszawa"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Zespół Szkół Samochodowych",
            //    PseudoName = "ZSS",
            //    NamedBy = "Sportowców Ziemii Szczecińskiej",
            //    City = new City { Name = "Szczecin" },
            //    Country = "PL",
            //    Adrress = "Małopolska 22",
            //    Tag = "zssszczecin"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Zespół Szkół Specjalnych nr 2",
            //    PseudoName = "ZSS2",
            //    NamedBy = null,
            //    City = new City { Name = "Gdańsk" },
            //    Country = "PL",
            //    Adrress = "Witastwosza 23",
            //    Tag = "zss2gdańsk"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Sopocka Szkoła Montessori",
            //    PseudoName = "SSP",
            //    NamedBy = null,
            //    City = new City { Name = "Sopot" },
            //    Country = "PL",
            //    Adrress = "Tatrzańska 19",
            //    Tag = "sspsopot"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Szkoła Podstawowa nr 84",
            //    PseudoName = "SP",
            //    NamedBy = "Ruch Oporu Pokoju",
            //    City = new City { Name = "Wrocław" },
            //    Country = "PL",
            //    Adrress = "Górnickiego 20",
            //    Tag = "spwroclaw"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "XII Liceum Ogólnokształcące",
            //    PseudoName = "XIILO",
            //    NamedBy = "M. Skłodwska-Curie",
            //    City = new City { Name = "Poznań" },
            //    Country = "PL",
            //    Adrress = "Kutrzeby 8",
            //    Tag = "XIIlieceumogolnoksztalcace"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Wyższa Szkoła Techniczna",
            //    PseudoName = "WST",
            //    NamedBy = null,
            //    City = new City { Name = "Katowice" },
            //    Country = "PL",
            //    Adrress = "Rolna 43",
            //    Tag = "wyzszaszkolatechniczna"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Śląski Uniwersytet Medyczny",
            //    PseudoName = "ŚUM",
            //    NamedBy = null,
            //    City = new City { Name = "Katowice" },
            //    Country = "PL",
            //    Adrress = "Poniatowskiego 15",
            //    Tag = "slaskiuniwerekmedyczny"
            //});
            //app_configuration.Add(new School
            //{
            //    OfficialName = "Szkoła Podstawowa nr 4",
            //    PseudoName = "SP",
            //    NamedBy = "T. Kościuszki. Filla",
            //    City = new City { Name = "Katowice" },
            //    Country = "PL",
            //    Adrress = "Józefowska 52/54",
            //    Tag = "szkolapodtawowakatowice"
            //});
            //app_configuration.SaveChanges();
        }
    }
}
