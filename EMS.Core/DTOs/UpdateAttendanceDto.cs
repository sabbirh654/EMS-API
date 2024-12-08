using EMS.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class UpdateAttendanceDto
{
    [Required(ErrorMessage = "Check-in time is required.")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    public TimeSpan CheckInTime { get; set; }

    [Required(ErrorMessage = "Check-out time is required.")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format.")]
    [CheckOutTimeValidation(nameof(CheckInTime))]
    public TimeSpan CheckOutTime { get; set; }
}
