using madwords.Models;
using madwords.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace madwords
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
            // services.AddResponseCaching();
            services.AddControllers();

            services.AddControllersWithViews();
            // Inject our repositories into our controllers
            services.AddTransient<IMadwordRepository, MadwordRepository>(); // Generic types: Repository interface, Repository class
            services.AddTransient<IBlogPostRepository, BlogPostRepository>();
            services.AddDbContext<MadwordContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SQLServerConnection"]));


            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MadwordContext>()
                .AddDefaultTokenProviders();

            /*
            services.AddDataProtection()
                .SetApplicationName("docker")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));
            */

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MadwordContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            var serviceProvider = app.ApplicationServices;
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedData.Seed(context, userManager, roleManager);
        }
    }
}