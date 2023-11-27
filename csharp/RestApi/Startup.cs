// Startup.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestApi.Repositories;
using RestApi.Services;
using RestApi.Services.Implementations;

namespace RestApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<MongoDBConfig>(Configuration.GetSection("MongoDB"));
            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
                return new MongoClient(settings.ConnectionString);
            });
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
                var database = client.GetDatabase(settings.DatabaseName);

                var initializer = new DatabaseInitializer(client, settings);
                initializer.SyncDatabaseWithModels().GetAwaiter().GetResult();

                return database;
            });

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var database = services.GetRequiredService<IMongoDatabase>();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}