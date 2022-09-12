using IdentityManager.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
      {
          options.Password.RequiredLength = 10;
          options.Password.RequiredUniqueChars = 3;
          options.Password.RequireNonAlphanumeric = false;
      }
      )
    .AddEntityFrameworkStores<AppDbContext>();
app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.Run();
