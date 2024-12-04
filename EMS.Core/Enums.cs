namespace EMS.Core;

public static class Enums
{
    public enum OperationType
    {
        Add,
        Update,
        Delete
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
        PostgreSql,
        MongoDb
    }
}
