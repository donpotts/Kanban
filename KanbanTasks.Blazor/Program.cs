using KanbanTasks.Shared.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Main>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var SyncFusionKey = builder.Configuration.GetValue<string>("SyncFusionLicenseKey");

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncFusionKey);

builder.Services.AddBlazorServices(builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();
