namespace EMS.Core.Extensions;

public static class DateTimeExtensions
{
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return TimeOnly.FromDateTime(dateTime);
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }
}
