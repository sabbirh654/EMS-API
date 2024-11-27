using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Core.Mappers;

public static class EmployeeMapper
{
    public static Employee MapEmployeeAddDto(this AddEmployeeDto dto)
    {
        return new Employee
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            DepartmentId = dto.DepartmentId,
            DesignationId = dto.DesignationId
        };
    }

    public static Employee MapEmployeeUpdateDto(this UpdateEmployeeDto dto)
    {
        return new Employee
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
        };
    }
}
