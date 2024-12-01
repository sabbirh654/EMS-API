using EMS.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.RegisterFrameworkServices();

var app = builder.Build();

app.ConfigureFrameworkMiddlewares();
app.MapControllers();
app.Run();
