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

/*builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
    return new MongoClient(options.ConnectionString);
});
builder.Services.AddScoped<MongoDbContext>();*/

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("DatabaseOptions"));
builder.Services.AddDbContext<SaleFlowDbContext>(options =>
{
    var dbOptions = builder.Configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>();
    options.UseSqlServer(dbOptions?.ConnectionString); 
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
