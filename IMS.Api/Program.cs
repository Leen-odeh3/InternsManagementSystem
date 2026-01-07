using IMS.Application.DI;
using FluentValidation.AspNetCore;
using IMS.Infrastructure.DbInitilizer;
using IMS.Infrastructure.ServiceContainer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.addInfraDependancy(builder.Configuration).AddApplicationDependancy();

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitilizer>();
        await dbInitializer.Initialize();
    }
    catch (Exception ex)
    {
        Console.WriteLine("DB init failed:");
        Console.WriteLine(ex.ToString());
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
