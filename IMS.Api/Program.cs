using IMS.Application.DI;
using IMS.Infrastructure.DependancyInjection;
using IMS.Infrastructure.ServiceContainer;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .addInfraDependancy(builder.Configuration)
    .AddApplicationDependancy();
builder.Services.AddObservability(builder.Configuration);

var app = builder.Build();
app.UseObservability(); 
app.MapObservabilityEndpoints();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
