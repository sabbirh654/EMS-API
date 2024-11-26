using EMS.Repository.DatabaseProviders;
using EMS.Repository.Implementations;
using EMS.Repository.Interfaces;

namespace EMS.API;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDesignationRepository, DesignationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        return services;
    }
}
