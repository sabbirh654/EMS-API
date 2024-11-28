using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Services.Interfaces;

public interface IDesignationService
{
    Task<IEnumerable<Designation>?> GetAllDesignations();
    Task<Designation?> GetDesignationById(int id);
    Task AddDesignation(AddUpdateDesignationDto dto);
    Task UpdateDesignation(int id, AddUpdateDesignationDto dto);
    Task DeleteDesignation(int id);
}
