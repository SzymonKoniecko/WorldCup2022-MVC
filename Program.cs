using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Contexts;
using WorldCup2022_MVC.Interfaces;
using WorldCup2022_MVC.Respository;
using WorldCup2022_MVC.Services;

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
builder.Services.AddDbContext<MatchesContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<KnockoutStageContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<PromotedTeamsContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<SimulatedKnockoutPhaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<ITeamRespository, TeamRespository>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IGroupStageRespository, GroupStageRespository>();
builder.Services.AddTransient<IGroupStageService, GroupStageService>();
builder.Services.AddTransient<IMatchesRespository, MatchesRespository>();
builder.Services.AddTransient<IMatchesService, MatchesService>();
builder.Services.AddTransient<IKnockoutStageRespository, KnockoutStageRespository>();
builder.Services.AddTransient<IKnockoutStageService, KnockoutStageService>();
builder.Services.AddTransient<IPromotedTeamsRespository, PromotedTeamsRespository>();
builder.Services.AddTransient<IPromotedTeamsService, PromotedTeamsService>();
builder.Services.AddTransient<ISimulatedKnockoutPhaseRespository, SimulatedKnockoutPhaseRespository>();
builder.Services.AddTransient<ISimulatedKnockoutPhaseService, SimulatedKnockoutPhaseService>();
builder.Services.AddControllersWithViews();



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var _context = services.GetRequiredService<GroupContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context.Database.Migrate();
        }
    }
    var _context1 = services.GetRequiredService<GroupStageContext>();
    if (_context1.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context1.Database.Migrate();
        }
    }
    var _context2 = services.GetRequiredService<KnockoutStageContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context2.Database.Migrate();
        }
    }
    var _context3 = services.GetRequiredService<MatchesContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context3.Database.Migrate();
        }
    }
    var _context4 = services.GetRequiredService<PromotedTeamsContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context4.Database.Migrate();
        }
    }
    var _context5 = services.GetRequiredService<SimulatedKnockoutPhaseContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context5.Database.Migrate();
        }
    }
    var _context6 = services.GetRequiredService<TeamContext>();
    if (_context.Database.CanConnect())
    {
        var pendingMigrations = _context.Database.GetPendingMigrations();
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _context6.Database.Migrate();
        }
    }
}
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
                name: "PlayGroup",
                pattern: "PlayGroup",
                defaults: new { controller = "PlayGroup", action = "RedirectToPlay" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
