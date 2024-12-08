using EMS.API.Filters;
using EMS.Repository.DatabaseProviders.Implementations;
using EMS.Repository.DatabaseProviders.Interfaces;
using EMS.Repository.Implementations;
using EMS.Repository.Interfaces;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;

namespace EMS.API;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDesignationRepository, DesignationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IOperationLogRepository, OperationLogRepository>();

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDesignationService, DesignationService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAttendanceService, AttendanceService>();

        services.AddScoped<SqlServerExceptionHandler>();
        services.AddScoped<PostgreSqlExceptionHandler>();
        services.AddScoped<MongoDbExceptionHandler>();

        services.AddScoped<IDatabaseExceptionHandlerFactory, DatabaseExceptionHandlerFactory>();

        return services;
    }

    public static IServiceCollection RegisterFrameworkServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => c.SupportNonNullableReferenceTypes());

        return services;
    }
}
