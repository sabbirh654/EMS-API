﻿using DnsClient.Internal;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Mappers;
using EMS.Core.Models;
using EMS.Repository.Implementations;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ILogger<AttendanceService> _logger;
    private readonly IEmployeeRepository _employeeRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository, ILogger<AttendanceService> logger, IEmployeeRepository employeeRepository)
    {
        _attendanceRepository = attendanceRepository;
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    public async Task<ApiResult> AddAttendance(AddAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceAddDto();

        try
        {
            var employee = _employeeRepository.GetByIdAsync(dto.EmployeeId);

            if (employee.Result.Result == null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, "Employee not found or deleted to add attendance");
            }

            return await _attendanceRepository.AddAsync(attendance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendanceService), nameof(AddAttendance), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_ATTENDANCE_ERROR);
        }
    }

    public async Task<ApiResult> DeleteAttendance(int id)
    {
        try
        {
            return await _attendanceRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendanceService), nameof(DeleteAttendance), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR);
        }
    }

    public async Task<ApiResult> GetAllAttendance(AttendanceFilter filter)
    {
        try
        {
            return await _attendanceRepository.GetAllAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendanceService), nameof(GetAllAttendance), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
        }
    }

    public Task<ApiResult> GetAttendanceById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResult> GetEmployeeAttendance(int employeeId)
    {
        try
        {
            return await _attendanceRepository.GetAllByIdAsync(employeeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendanceService), nameof(GetEmployeeAttendance), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
        }
    }

    public async Task<ApiResult> UpdateAttendance(int id, UpdateAttendanceDto dto)
    {
        Attendance attendance = dto.MapAttendanceUpdateDto();
        attendance.Id = id;

        try
        {
            return await _attendanceRepository.UpdateAsync(attendance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(AttendanceService), nameof(UpdateAttendance), ex.Message));

            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR);
        }
    }
}
