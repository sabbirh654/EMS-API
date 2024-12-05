﻿using EMS.API.Models;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        private readonly ILogger<DesignationsController> _logger;

        public DesignationsController(IDesignationService designationService, ILogger<DesignationsController> logger)
        {
            _designationService = designationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesignations()
        {
            try
            {
                var result = await _designationService.GetAllDesignations();

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(DesignationsController), nameof(GetAllDesignations), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            try
            {
                var result = await _designationService.GetDesignationById(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(GetDesignationById), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDesignation([FromBody] AddUpdateDesignationDto dto)
        {
            try
            {
                var result = await _designationService.AddDesignation(dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(AddDesignation), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_DESIGNATION_ERROR));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] AddUpdateDesignationDto dto)
        {
            try
            {
                var result = await _designationService.UpdateDesignation(id, dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(UpdateDesignation), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
                var result =  await _designationService.DeleteDesignation(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(DeleteDesignation), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR));
            }
        }
    }
}
