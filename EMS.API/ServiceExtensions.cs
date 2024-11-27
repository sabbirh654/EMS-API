using EMS.Repository.DatabaseProviders;
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

        return services;
    }
}
