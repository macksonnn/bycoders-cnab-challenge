using Cnab.Application.Interfaces;
using Cnab.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICnabImportService, CnabImportService>();
        services.AddScoped<IStoreService, StoreService>();

        return services;
    }
}

