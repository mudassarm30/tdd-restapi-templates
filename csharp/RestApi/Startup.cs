using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RestApi.Models;
using RestApi.Repositories;
using RestApi.Repositories.Implementations;
using RestApi.Services;
using RestApi.Services.Implementations;

namespace RestApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

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

                return database;
            });

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped(s =>
            {
                var passwordHasher = s.GetRequiredService<IPasswordHasher<User>>();
                var client = s.GetRequiredService<IMongoClient>();
                var settings = s.GetRequiredService<IOptions<MongoDBConfig>>();
                var collectionName = DatabaseInitializer.GetCollectionName<User>();

                return new UserRepository(client, settings, passwordHasher);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<JwtService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var client = serviceProvider.GetService<IMongoClient>();
            var settings = serviceProvider.GetService<IOptions<MongoDBConfig>>().Value;

            var initializer = new DatabaseInitializer(client, settings);
            initializer.SyncDatabaseWithModels().GetAwaiter().GetResult();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}