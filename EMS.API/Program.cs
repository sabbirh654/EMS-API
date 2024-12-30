using EMS.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.RegisterFrameworkServices();
builder.Services.RegisterJwtAuthentication(builder);
builder.Services.RegisterSwaggerConfiguration();

var app = builder.Build();

app.ConfigureFrameworkMiddlewares();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
