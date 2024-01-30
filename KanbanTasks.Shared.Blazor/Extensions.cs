using KanbanTasks.Shared.Blazor.Authorization;
using KanbanTasks.Shared.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;
using System.Security.Authentication;

namespace KanbanTasks.Shared.Blazor;

public static class Extensions
{
    public static void AddBlazorServices(this IServiceCollection services, string baseAddress)
    {
        services.AddScoped<AppService>();

        services.AddScoped(sp
            => new HttpClient { BaseAddress = new Uri(baseAddress) });

        services.AddAuthorizationCore();
        services
            .AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();
               
        services.AddSyncfusionBlazor();
    }
}
