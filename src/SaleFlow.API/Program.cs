using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SaleFlow.Repository.Configurations;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Repository.Repositories;
using SaleFlow.Service.Profiles;
using SaleFlow.Service.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("DatabaseOptions"));

builder.Services.AddDbContext<SaleFlowDbContext>(options =>
{
    var dbOptions = builder.Configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>();
    options.UseNpgsql(dbOptions?.ConnectionString);
});

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetSalesQueryHandler).Assembly));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaleFlow.API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SaleFlowDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaleFlow.API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
