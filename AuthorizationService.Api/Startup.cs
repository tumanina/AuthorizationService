using AuthorizationService.Business;
using AuthorizationService.Repositories;
using AuthorizationService.Repositories.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationService.Api
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
            var connectionString = Configuration.GetConnectionString("AuthDBConnectionString");

            var dbOptions = new DbContextOptionsBuilder<AuthDBContext>();
            dbOptions.UseSqlServer(connectionString);
            services.AddSingleton<IAuthDBContextFactory>(t => new AuthDBContextFactory(dbOptions));


            services.AddSingleton<IAuthDBContext, AuthDBContext>();

            services.AddSingleton<ISessionRepository, SessionRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{id}");
            });
        }
    }
}