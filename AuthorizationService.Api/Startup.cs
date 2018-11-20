using AuthorizationService.Business;
using AuthorizationService.Repositories;
using AuthorizationService.Repositories.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace AuthorizationService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            services.AddSingleton<IAuthService, AuthService>();

            services.AddMvc();
            services.AddMvcCore().AddJsonFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Authorization API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("Default", new ApiKeyScheme
                {
                    Description = "Authorization header Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Warning));
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization Service V1");
            });

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{id}");
            });
        }
    }
}