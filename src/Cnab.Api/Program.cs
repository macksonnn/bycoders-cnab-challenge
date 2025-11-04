using Cnab.Application;
using Cnab.Application.Interfaces;
using Cnab.Infrastructure;
using Cnab.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CNAB API - Macksonnn",
        Version = "v1",
        Description = "API para importação e consulta de transações CNAB"
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CnabDbContext>();
    dbContext.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CNAB API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors();
app.MapPost("/api/cnab/import", async (IFormFile file, ICnabImportService importService) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new { error = "Arquivo não fornecido ou vazio." });
    }

    try
    {
        using var stream = file.OpenReadStream();
        await importService.ImportAsync(stream);
        
        return Results.Ok(new 
        { 
            message = "Arquivo CNAB importado com sucesso.",
            fileName = file.FileName,
            fileSize = file.Length
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = $"Erro ao processar arquivo: {ex.Message}" });
    }
})
.WithName("ImportCnab")
.WithDescription("Importa um arquivo CNAB contendo transações financeiras")
.WithOpenApi()
.DisableAntiforgery();
app.MapGet("/api/stores", async (IStoreService storeService) =>
{
    try
    {
        var stores = await storeService.GetStoresWithBalanceAsync();
        return Results.Ok(stores);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = $"Erro ao buscar lojas: {ex.Message}" });
    }
})
.WithName("GetStores")
.WithDescription("Lista todas as lojas com suas transações e saldo consolidado")
.WithOpenApi();
app.MapGet("/", () => Results.Ok(new 
{ 
    service = "CNAB API",
    version = "1.0",
    status = "running"
}))
.WithName("HealthCheck")
.WithDescription("Verifica o status da API")
.WithOpenApi();

app.Run();

public partial class Program { }

