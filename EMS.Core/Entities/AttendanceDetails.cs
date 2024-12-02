namespace EMS.Core.Entities;

public class AttendanceDetails
{
    public DateTime AttendanceDate { get; set; }
    public TimeSpan EarliestCheckIn { get; set; }
    public TimeSpan LatestCheckOut { get; set; }
}
