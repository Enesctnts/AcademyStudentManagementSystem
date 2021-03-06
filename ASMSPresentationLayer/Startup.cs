using ASMSBusinessLayer.ContractsBLL;
using ASMSBusinessLayer.EmailService;
using ASMSBusinessLayer.ImplementationsBLL;
using ASMSDataAccessLayer;
using ASMSEntityLayer.IdentityModels;
using ASMSEntityLayer.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASMSDataAccessLayer.ContactsDAL;
using ASMSDataAccessLayer.ImplementationsDAL;

namespace ASMSPresentationLayer
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
            //Aspnet Core'un ConnectionString ba?lant?s? yapabilmesi i?in 
            //yap?land?rma servislerine dbcontext nesnesini eklemesi gerekir
            
            services.AddDbContext<MyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));


            
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();//Proje ?al???rken razor sayfalar?nda yap?lan de?i?ikler an?nda yans?mas? i?in eklendi.



            services.AddRazorPages(); // razor sayfalar? i?in
            services.AddMvc();
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromSeconds(20));
            // oturum zaman?

            //**************************************//
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_@.";
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();

            //Mapleme eklendi.
            services.AddAutoMapper(typeof(Maps));

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<ICityBusinessEngine, CityBusinessEngine>();
            services.AddScoped<IStudentBusinessEngine, StudentBusinessEngine>();
            services.AddScoped<IUsersAddressBusinessEngine, UsersAddressBusinessEngine>();
            services.AddScoped<IDistrictBusinessEngine, DistrictBusinessEngine>();
            services.AddScoped<INeighbourhoodBusinessEngine, NeighbourhoodBusinessEngine>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RoleManager<AppRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles(); // wwwroot klas?r?n?n eri?imi i?indir.
            app.UseRouting(); // Controller/Action/Id 
            app.UseSession(); // Oturum mekanizmas?n?n kullan?lmas? i?in
            app.UseAuthentication(); // Login Logout i?lemlerinin gerektirti?i oturum i?leyi?lerini kullanabilmek i?in.
            app.UseAuthorization(); // [Authorize] attribute i?in (yetki)

            //rolleri olu?turacak static metod ?a?r?ld?
            CreateDefaultData.CreateData.Create(roleManager);

            //MVC ile ayn? kod blo?u endpoint'in mekanizmas?n?n nas?l olaca?? belirleniyor
            app.UseEndpoints(endpoints =>
            {
                //Area i?in yap?yoruz alttakini. ?lk area i?lemini yaz?yoruz.o y?zden areay? ?ne yaz?yoz ilk areaya bak diyoz. yoksa ?al??m?yor sistem
                endpoints.MapAreaControllerRoute(
                  "management",
                  "management",
                  "management/{controller=Admin}/{action=Login}/{id?}"
                  );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                

                
            });

            

        }
    }
}
