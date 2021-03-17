using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi
{
    public class Startup
    {
        private readonly string FrontendOrigins="FrontendOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: FrontendOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000");
                                      builder.AllowAnyHeader();
                                      builder.AllowCredentials();
                                  });
            });
            services.AddControllers();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SharyApi", Version = "v1" });
            });
            

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<Shary2Context>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.GetConnectionString("Shary"));
            });
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IStationRepository, StationRepository>();
            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IIndividualRepository, IndividualRepository>();
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SharyApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(FrontendOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
