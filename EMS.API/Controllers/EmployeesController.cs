using ClosedXML.Excel;
using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeService.GetAllEmployees();

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(GetAllEmployees), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Employee ID must be a positive integer."));
            }

            try
            {
                var result = await _employeeService.GetEmployeeById(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(GetEmployeeById), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpGet("download-xlsx")]
        public async Task<IActionResult> GetEmployeeDataAsXlsxFile()
        {
            var res = await _employeeService.GetAllEmployees();
            var employees = res.Result as List<EmployeeDetails>;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Phone Number";
            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Cell(1, 6).Value = "Birth Date";
            worksheet.Cell(1, 7).Value = "Department";
            worksheet.Cell(1, 8).Value = "Designation";

            for (int i = 0; i < employees?.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = employees[i].Id;
                worksheet.Cell(i + 2, 2).Value = employees[i].Name;
                worksheet.Cell(i + 2, 3).Value = employees[i].Email;
                worksheet.Cell(i + 2, 4).Value = employees[i].PhoneNumber;
                worksheet.Cell(i + 2, 5).Value = employees[i].Address;
                worksheet.Cell(i + 2, 6).Value = employees[i].BirthDate;
                worksheet.Cell(i + 2, 7).Value = employees[i].Department;
                worksheet.Cell(i + 2, 8).Value = employees[i].Designation;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var fileBytes = stream.ToArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeList.xlsx");
        }

        [HttpGet("download-csv")]
        public async Task<IActionResult> GetEmployeeDataAsCsvFile()
        {
            var res = await _employeeService.GetAllEmployees();
            var employees = res.Result as List<EmployeeDetails>;

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("ID,Name,Email, Phone Number, Address, Birth Date, Department, Designation"); // Header row

            foreach (var emp in employees)
            {
                csvBuilder.AppendLine($"{emp.Id},{emp.Name},{emp.Email},{emp.PhoneNumber}, {emp.Address}, {emp.BirthDate}, {emp.Department}, {emp.Designation}");
            }

            var csvData = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(csvData, "text/csv", "EmployeeList.csv");

        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.AddEmployee(dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(AddEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_EMPLOYEE_ERROR));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Employee ID must be a positive integer."));
            }

            try
            {
                var result = await _employeeService.UpdateEmployee(id, dto);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(UpdateEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR, "Employee ID must be a positive integer."));
            }

            try
            {
                var result = await _employeeService.DeleteEmployee(id);

                if (!result.IsSuccess)
                {
                    return StatusCode(result.ErrorCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage.GetErrorMessage(nameof(EmployeesController), nameof(DeleteEmployee), ex.Message));

                return StatusCode(500, ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR));
            }
        }
    }
}
