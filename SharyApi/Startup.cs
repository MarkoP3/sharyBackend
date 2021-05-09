using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Helpers;
using SharyApi.Hubs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SharyApi
{
    public class Startup
    {
        private readonly string FrontendOrigins = "FrontendOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {


                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: FrontendOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(Configuration["Frontend"]);
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
            services.AddSignalR();
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
            else
            {
                app.ConfigureErrorHandling();
            }
            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseCors(FrontendOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TestHub>("/admin");
                endpoints.MapHub<TestHub>("/individual");
                endpoints.MapHub<TestHub>("/station");
                endpoints.MapHub<TestHub>("/business");

            });
        }
    }
}
