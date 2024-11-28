using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Core.Mappers;

public static class DepartmentMapper
{
    public static Department MapDepartmentAddUpdateDto(this AddUpdateDepartmentDto dto)
    {
        return new Department
        {
            Name = dto.Name
        };
    }
}
