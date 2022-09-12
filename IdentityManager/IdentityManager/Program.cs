using IdentityManager.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.Run();
