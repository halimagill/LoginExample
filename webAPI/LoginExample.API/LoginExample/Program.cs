using System.Text;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using LoginExample;
using LoginExample.Data;
using LoginExample.Data.Models;
using LoginExample.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLamar()
    .RegisterServices()
    .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ApiCorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
