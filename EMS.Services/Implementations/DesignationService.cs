using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Mappers;
using EMS.Core.Models;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _designationRepository;
    private readonly ILogger<DesignationService> _logger;

    public DesignationService(IDesignationRepository designationRepository, ILogger<DesignationService> logger)
    {
        _designationRepository = designationRepository;
        _logger = logger;
    }
    public async Task<ApiResult> AddDesignation(AddUpdateDesignationDto dto)
    {
        Designation desingation = dto.MapDesignationAddUpdateDto();

        try
        {
            return await _designationRepository.AddAsync(desingation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationService), nameof(AddDesignation), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DESIGNATION_ERROR);
        }
    }

    public async Task<ApiResult> DeleteDesignation(int id)
    {
        try
        {
            var designation = _designationRepository.GetByIdAsync(id);

            if (designation.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Designation not found or deleted");
            }

            return await _designationRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationService), nameof(DeleteDesignation), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR);
        }
    }

    public async Task<ApiResult> GetAllDesignations()
    {
        try
        {
            return await _designationRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationService), nameof(GetAllDesignations), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
        }
    }

    public async Task<ApiResult> GetDesignationById(int id)
    {
        try
        {
            var designation = _designationRepository.GetByIdAsync(id);

            if (designation.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "designation not found or deleted");
            }

            return await _designationRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationService), nameof(GetDesignationById), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
        }
    }

    public async Task<ApiResult> UpdateDesignation(int id, AddUpdateDesignationDto dto)
    {
        Designation designation = dto.MapDesignationAddUpdateDto();
        designation.Id = id;

        try
        {
            var des = _designationRepository.GetByIdAsync(id);

            if (des.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Designation not found or deleted");
            }

            return await _designationRepository.UpdateAsync(designation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationService), nameof(UpdateDesignation), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR);
        }
    }
}
