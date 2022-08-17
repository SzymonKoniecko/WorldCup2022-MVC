using WorldCup2022_MVC.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Respository;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Services;
using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Models;
using WorldCup2022_MVC.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("WorldCup2022");
builder.Services.AddDbContext<GroupContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<GroupStageContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<TeamContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<ITeamRespository, TeamRespository>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
