namespace EMS.Core;

public static class Enums
{
    public enum OperationType
    {
        ADD,
        UPDATE,
        DELETE
    }

    public enum EntityName
    {
        Employee,
        Department,
        Designation,
        Attendance
    }

    public enum DatabaseType
    {
        SqlServer,
        PostgreSql
    }
}
