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
    private readonly ILogger<AttendanceService> _logger;

    public AttendanceService(IAttendanceRepository attendanceRepository, ILogger<AttendanceService> logger)
    {
        _attendanceRepository = attendanceRepository;
        _logger = logger;
    }

    public async Task AddAttendance(AddAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceAddDto();

        try
        {
            await _attendanceRepository.AddAsync(attendance);
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
        try
        {
            await _attendanceRepository.DeleteAsync(id);
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

    public async Task<IEnumerable<AttendanceDetails>?> GetEmployeeAttendance(int employeeId)
    {
        try
        {
            var result = await _attendanceRepository.GetAllByIdAsync(employeeId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Service error in {nameof(EmployeeService)} at {nameof(GetAllAttendance)} function");
            throw;
        }
    }

    public async Task UpdateAttendance(int id, UpdateAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceUpdateDto();
        attendance.Id = id;

        try
        {
            await _attendanceRepository.UpdateAsync(attendance);
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
