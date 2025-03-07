using System;
using System.Text;
using Lamar;
using LoginExample.BSN.Interfaces;
using LoginExample.BSN.Services;
using LoginExample.Data;
using LoginExample.Data.Models;
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
                string corsUrl = Configuration.GetValue<string>("CorsUrl");
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(connectionString
                                        , x => x.MigrationsAssembly("LoginExample.Data.Migrations")));
                services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<AppUser>>();

                services.Configure<IdentityOptions>(options =>
                {
                    configuration.GetSection("Identity:Options").Bind(options);
                });
                
                services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
                {
                    builder.WithOrigins(corsUrl).AllowAnyMethod().AllowAnyHeader();
                }));
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
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
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
                        Description = Configuration["Jwt:Secret"],
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
                services.For<IUserManagerService>().Use(x => 
                {
                    var userMrg = x.GetInstance<UserManager<AppUser>>();
                    var uow = x.GetInstance<IUnitOfWork<IAppIdentityDbContext>>();
                    return new UserManagerService(userMrg, uow);
                });
                services.For<IAuthService>().Use(x =>
                {
                    string privateKey = Configuration["Jwt:Secret"].ToString();
                    var signInMrg = x.GetInstance<SignInManager<AppUser>>();
                    var userManagerService = x.GetInstance<IUserManagerService>();

                    return new AuthService(privateKey, signInMrg, userManagerService);
                 });
        });


            return builder;
        }
    }
}