using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Core.Mappers;

public static class DesignationMapper
{
    public static Designation MapDesignationAddUpdateDto(this AddUpdateDesignationDto dto)
    {
        return new Designation
        {
            Name = dto.Name
        };
    }
}
