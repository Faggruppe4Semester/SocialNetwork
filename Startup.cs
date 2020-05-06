using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork
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
            services.Configure<SocialNetworkDatabaseSettings>(
                Configuration.GetSection(nameof(SocialNetworkDatabaseSettings)));

            services.AddSingleton<ISocialNetworkDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SocialNetworkDatabaseSettings>>().Value);

            services.AddSingleton(service => 
                new GenericService<User>(
                    (SocialNetworkDatabaseSettings)service.GetRequiredService(typeof(ISocialNetworkDatabaseSettings)), 
                    Configuration[nameof(SocialNetworkDatabaseSettings) + ":UserCollectionName"]) //Remember the colon, to notify the nested value of the SocialNetworkDatabaseSettings JSON object.
            ); //How to initialize GenericService as Singleton.

            services.AddSingleton(service =>
                    new GenericService<Circle>(
                        (SocialNetworkDatabaseSettings)service.GetRequiredService(typeof(ISocialNetworkDatabaseSettings)),
                        Configuration[nameof(SocialNetworkDatabaseSettings) + ":CircleCollectionName"])
            );

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
