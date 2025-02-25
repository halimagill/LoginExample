using System;
using System.Text;
using Lamar;
using LoginExample.Data;
using LoginExample.Data.Models;
using LoginExample.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LoginExample
{
    public static class RegisterDependentServices
    {
        public static IHostBuilder RegisterServices(this IHostBuilder builder)
        {
            builder.ConfigureContainer<ServiceRegistry>((context, services) =>
            {
                // Also exposes Lamar specific registrations
                // and functionality
                services.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.Assembly("LoginExample.Data");
                    s.Assembly("LoginExample.BSN");
                    s.WithDefaultConventions();
                });

                var configuration = context.Configuration;
                IConfigurationRoot Configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(connectionString
                                        , x => x.MigrationsAssembly("LoginExample.Data.Migrations")));
                services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

                // Add services to the container.
                //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
                services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),
                        ValidateIssuer = false, //probably should check into this
                        ValidateAudience = false,
                    };
                });

                services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(c =>
                {
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "JWT Authentication",
                        Description = "Enter your JWT token in this field",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    };

                    c.AddSecurityDefinition("Bearer", securityScheme);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    };

                    c.AddSecurityRequirement(securityRequirement);
                });

                services.For<AppIdentityDbContext>().Use(x => new AppIdentityDbContext(connectionString));
                services.For<IAppIdentityDbContext>().Use(x => x.GetInstance<AppIdentityDbContext>());
                services.For<IUnitOfWork<IAppIdentityDbContext>>().Use<UnitOfWork>().Scoped();

            });


            return builder;
        }
    }
}