using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Wolfden.Server.Data;
using Wolfden.Server.Models;
using Serilog;
using Wolfden.Server.Hubs;
using Wolfden.Server.Hubs.Lupus;
using Lupus.Factories;
using Lupus;
using Newtonsoft.Json;
using Lupus.Other.MapLoading;
using Wolfden.Server.Other;
using System.Drawing;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("wwwroot/logs"));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
//                options.SignIn.RequireConfirmedAccount = false;
//                options.Password.RequireDigit = false;
//                options.Password.RequireLowercase = false;
//                options.Password.RequireNonAlphanumeric = false;
//                options.Password.RequireUppercase = false;
//                options.Password.RequiredLength = 3;
//                options.Password.RequiredUniqueChars = 1;
//            })
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//app.UseIdentityServer();
//app.UseAuthentication();
//app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseEndpoints(endpoints =>
{
    //endpoints.MapHub<LobbyHub<ILobbyClient>>("/lobbyHub");
    endpoints.MapHub<LupusLobbyHub>("/lupusLobbyHub");
    endpoints.MapHub<LupusGameHub>("/lupusgamehub");
});


var players = new List<PlayerInfo>() { new PlayerInfo() { Id = "Guest_Test", Color = KnownColor.Red, Name = "TestPlayer" } };
var mapString = File.ReadAllText(app.Environment.WebRootPath + "/game/maps/Duel 2/Duel 2.json");
var jsonMap = JsonConvert.DeserializeObject<JsonMap>(mapString);
var id = new Guid("4e0f20d6-3cc2-456c-9a04-e400d7f5a634");
var game = new Game(id, players, jsonMap);
game.Initialize();
ConcurrencyObjects.AddObject(game.Id, game);


app.Run();

