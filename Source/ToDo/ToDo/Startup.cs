using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;

namespace ToDo
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
            // Criado na aula 5
            var connString = Configuration.GetConnectionString("DefaultConnection");

            services
                .AddDbContext<ApplicationDbContext>
                    (options => options
                        .UseSqlite(connString));
            // Fim

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // Criado na aula 4
            // Removido o Fake e deixado a class original TodoItemService que tem conexão com banco
     //       services.AddSingleton<IToDoItemService, ToDoItemService>();
            services.AddScoped<IToDoItemService, ToDoItemService>();
            // Fim

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                /* aula 11 */
                EnsureRoleAsync(roleManager).Wait();
                EnsureTestAdminAsync(userManager).Wait();
                /* fim */
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /* aula 11 */
        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists)
                return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null)
                return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, "NotSecure123!!");

            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
        /* fim */

    }
}
