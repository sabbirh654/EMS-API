using DnsClient.Internal;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Exceptions;
using EMS.Core.Mappers;
using EMS.Repository.Implementations;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IOperationLogRepository _operationLogRepository;
    private readonly ILogger<AttendanceService> _logger;

    public AttendanceService(IAttendanceRepository attendanceRepository, IOperationLogRepository operationLogRepository, ILogger<AttendanceService> logger)
    {
        _attendanceRepository = attendanceRepository;
        _operationLogRepository = operationLogRepository;
        _logger = logger;
    }

    public async Task AddAttendance(AddAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceAddDto();

        OperationLog log = new OperationLog("ADD", "Attendance", "", "Attendance has been added");

        try
        {
            await _attendanceRepository.AddAsync(attendance);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(AttendanceService)} at {nameof(AddAttendance)} function");
            throw new ServiceException(ex.Message, ex.ErrorCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(AttendanceService)} at {nameof(AddAttendance)} function");
            throw;
        }
    }

    public async Task DeleteAttendance(int id)
    {
        OperationLog log = new OperationLog("DELETE", "Attendance", $"{id}", $"Attendance has been deleted with Id = {id}");

        try
        {
            await _attendanceRepository.DeleteAsync(id);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(AttendanceService)} at {nameof(DeleteAttendance)} function");
            throw;
        }
    }

    public async Task<IEnumerable<Attendance>?> GetAllAttendance(AttendanceFilter filter)
    {
        try
        {
            var result = await _attendanceRepository.GetAllAsync(filter);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(GetAllAttendance)} function");
            throw;
        }
    }

    public Task<Attendance?> GetAttendanceById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAttendance(int id, UpdateAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceUpdateDto();
        attendance.Id = id;

        OperationLog log = new("UPDATE", "Attendance", $"{id}", $"Attendance has been updated with Id = {id}");

        try
        {
            await _attendanceRepository.UpdateAsync(attendance);
            await _operationLogRepository.AddLogAsync(log);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(AttendanceService)} at {nameof(AddAttendance)} function");
            throw new ServiceException(ex.Message, ex.ErrorCode);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(AttendanceRepository)} at {nameof(UpdateAttendance)} function");
            throw;
        }
    }
}
