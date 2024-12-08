namespace EMS.Core.Helpers;

public static class ErrorMessage
{
    public const string ADD_EMPLOYEE_ERROR = "Failed to add employee";
    public const string DELETE_EMPLOYEE_ERROR = "Failed to delete employee";
    public const string UPDATE_EMPLOYEE_ERROR = "Failed to update employee";
    public const string GET_EMPLOYEE_ERROR = "An error occured while fetching employee data";

    public const string ADD_DESIGNATION_ERROR = "Failed to add designation";
    public const string DELETE_DESIGNATION_ERROR = "Failed to delete designation";
    public const string UPDATE_DESIGNATION_ERROR = "Failed to update designation";
    public const string GET_DESIGNATION_ERROR = "An error occured while fetching designation data";

    public const string ADD_DEPARTMENT_ERROR = "Failed to add department";
    public const string DELETE_DEPARTMENT_ERROR = "Failed to delete department";
    public const string UPDATE_DEPARTMENT_ERROR = "Failed to update department";
    public const string GET_DEPARTMENT_ERROR = "An error occured while fetching department data";

    public const string ADD_ATTENDANCE_ERROR = "Failed to add attendance";
    public const string DELETE_ATTENDANCE_ERROR = "Failed to delete attendance";
    public const string UPDATE_ATTENDANCE_ERROR = "Failed to update attendance";
    public const string GET_ATTENDANCE_ERROR = "An error occured while fetching attendance data";

    public const string ADD_LOG_ERROR = "Failed to add log";
    public const string GET_LOG_ERROR = "An error occured while fetching log data";

    public const string VALIDATION_ERROR = "Validation failed. See errors for details.";

    public const string DEPARTMENT_NOT_FOUND = "The department you are trying to add is not found";

    public static string GetErrorMessage(string layerName, string methodName, string errorMessage)
    {
        return $"An error occured in {layerName} -> {methodName}.\n Error message: {errorMessage}";
    }
}
