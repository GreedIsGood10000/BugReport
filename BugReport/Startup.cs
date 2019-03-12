using BugReport.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugReport
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
          //  string connection = @"Server=KOMP777\SQLEXPRESS;Database=BugTracker;Trusted_Connection=True;ConnectRetryCount=0";
            string connection = Configuration.GetConnectionString("db");
          //  string connection = @"server=localhost;Database=myDataBase;user=root;password=Passw0rd;";
            services.AddDbContext<BugTrackerContext>
                (options => options.UseMySQL(connection));
              //  (options => options.UseInMemoryDatabase(connection));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
