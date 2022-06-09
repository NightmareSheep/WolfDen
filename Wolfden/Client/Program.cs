using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Wolfden.Client;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Wolfden.Client.Other;
using LupusBlazor.Pixi;
using LupusBlazor.Pixi.LupusPixi;
using LupusBlazor.Audio.Json;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddHttpClient("Wolfden.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
//    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

//builder.Services.AddHttpClient("Wolfden.ServerAPI.Anonymous", client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//});

//// Supply HttpClient instances that include access tokens when making requests to the server project
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Wolfden.ServerAPI"));

builder.Services.AddApiAuthorization();

var host = builder.Build();

var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var http = host.Services.GetRequiredService<HttpClient>();

// Initialization
await JavascriptHelperModule.Initialize(jsRuntime);
await PixiApplicationModule.Initialize(jsRuntime);
await ViewportModule.Initialize(jsRuntime);

var javascriptHelper = JavascriptHelperModule.Instance;
javascriptHelper.SetJavascriptProperty(new string[] { "PIXI", "settings", "SCALE_MODE" }, 2);
Console.WriteLine("Loading resources");
await jsRuntime.InvokeVoidAsync("PIXI.Loader.shared.add", "sprites", "/Images/sprites.json");

var pixiModule = PixiApplicationModule.Instance;
await pixiModule.LoadResources();
PixiFilters.Initialize(jsRuntime);

var name = await localStorage.GetItemAsync<string>("name");
var id = await localStorage.GetItemAsync<string>("id");



if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(id))
{
    var random = new Random();
    name = Statics.Names[random.Next(Statics.Names.Length)];
    id = "Guest_" + Guid.NewGuid().ToString();
    await localStorage.SetItemAsync("name", name);
    await localStorage.SetItemAsync("id", id);
}

var checkVolume = await localStorage.ContainKeyAsync("masterVolume");
if (!checkVolume)
{
    await localStorage.SetItemAsync("muted", false);
    await localStorage.SetItemAsync("masterVolume", 100);
    await localStorage.SetItemAsync("musicVolume", 10);
    await localStorage.SetItemAsync("effectsVolume", 100);
}

var audioJson = await http.GetFromJsonAsync<AudioJson>("mygameaudio.json");

var muted = await localStorage.GetItemAsync<bool>("muted");
var masterVolume = await localStorage.GetItemAsync<int>("masterVolume");
var musicVolume = await localStorage.GetItemAsync<int>("musicVolume");
var effectsVolume = await localStorage.GetItemAsync<int>("effectsVolume");

await localStorage.SetItemAsync("soundEnabled", false);
Statics.AudioPlayer = new(jsRuntime, muted, masterVolume, musicVolume, effectsVolume, audioJson);
Statics.AudioPlayer.ChangeMuteEvent += (object sender, bool muted) => { localStorage.SetItemAsync("muted", muted); };
Statics.AudioPlayer.ChangeMasterVolumeEvent += (object sender, int volume) => { localStorage.SetItemAsync("masterVolume", volume); };
Statics.AudioPlayer.ChangeMusicVolumeEvent += (object sender, int volume) => { localStorage.SetItemAsync("musicVolume", volume); };
Statics.AudioPlayer.ChangeEffectsVolumeEvent += (object sender, int volume) => { localStorage.SetItemAsync("effectsVolume", volume); };

Statics.AudioPlayer.Initialize();
// Initialization end


await host.RunAsync();
