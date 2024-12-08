using EMS.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class AddAttendanceDto
{
    [Required(ErrorMessage = "Employee ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Employee ID must be a positive number.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Check-in time is required.")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    public TimeSpan CheckInTime { get; set; }

    [Required(ErrorMessage = "Check-out time is required.")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    [CheckOutTimeValidation(nameof(CheckInTime))]
    public TimeSpan CheckOutTime { get; set; }
}
