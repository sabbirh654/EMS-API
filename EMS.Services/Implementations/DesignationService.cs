using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Mappers;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _designationRepository;
    private readonly ILogger<DesignationService> _logger;
    private readonly IOperationLogRepository _operationLogRepository;

    public DesignationService(IDesignationRepository designationRepository, ILogger<DesignationService> logger, IOperationLogRepository operationLogRepository)
    {
        _designationRepository = designationRepository;
        _logger = logger;
        _operationLogRepository = operationLogRepository;
    }
    public async Task AddDesignation(AddUpdateDesignationDto dto)
    {
        Designation desingation = dto.MapDesignationAddUpdateDto();

        OperationLog log = new OperationLog("ADD", "Designation", "", "New designation has been added");

        try
        {
            await _designationRepository.AddAsync(desingation);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DesignationService)} at {nameof(AddDesignation)} function");
            throw;
        }
    }

    public async Task DeleteDesignation(int id)
    {
        OperationLog log = new OperationLog("DELETE", "Designation", $"{id}", $"A desingation has been deleted with Id = {id}");

        try
        {
            await _designationRepository.DeleteAsync(id);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DesignationService)} at {nameof(DeleteDesignation)} function");
            throw;
        }
    }

    public async Task<IEnumerable<Designation>?> GetAllDesignations()
    {
        try
        {
            var result = await _designationRepository.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DesignationService)} at {nameof(GetAllDesignations)} function");
            throw;
        }
    }

    public async Task<Designation?> GetDesignationById(int id)
    {
        try
        {
            var result = await _designationRepository.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DesignationService)} at {nameof(GetDesignationById)} function");
            throw;
        }
    }

    public async Task UpdateDesignation(int id, AddUpdateDesignationDto dto)
    {
        Designation designation = dto.MapDesignationAddUpdateDto();
        designation.Id = id;

        OperationLog log = new("UPDATE", "Desingation", $"{id}", $"A designation has been updated with Id = {id}");

        try
        {
            await _designationRepository.UpdateAsync(designation);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(DesignationService)} at {nameof(UpdateDesignation)} function");
            throw;
        }
    }
}
