using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using MongoDb.Bson.NodaTime;
using Microsoft.AspNetCore.HttpOverrides;
using NodaTime.Serialization.JsonNet;
using NodaTime;
using Newtonsoft.Json.Serialization;

namespace WorkDuckyAPI
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
            services.AddControllers();

            services.AddCors(options => {
                options.AddPolicy("defaultPolicy", 
                    builder => {
                        builder.AllowAnyOrigin()
                            .WithHeaders("Content-Type", "Authorization")
                            .WithMethods("GET", "POST", "PUT", "OPTIONS");
                    });
            });

            services.AddMvc()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            NodaTimeSerializers.Register();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("defaultPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
