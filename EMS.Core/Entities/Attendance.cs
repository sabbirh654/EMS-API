﻿namespace EMS.Core.Entities;

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
}
