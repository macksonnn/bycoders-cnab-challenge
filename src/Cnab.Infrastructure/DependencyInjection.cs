using Cnab.Application.Interfaces;
using Cnab.Infrastructure.Data;
using Cnab.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        services.AddDbContext<CnabDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}

