using Bugtracker.WASM;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IMemberLocalStorage, MemberLocalStorage>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7051/api/") });

await builder.Build().RunAsync();
