using EMS.Core.DTOs;
using EMS.Core.Models;

namespace EMS.Services.Interfaces;

public interface IDesignationService
{
    Task<ApiResult> GetAllDesignations();
    Task<ApiResult> GetDesignationById(int id);
    Task<ApiResult> AddDesignation(AddUpdateDesignationDto dto);
    Task<ApiResult> UpdateDesignation(int id, AddUpdateDesignationDto dto);
    Task<ApiResult> DeleteDesignation(int id);
}
