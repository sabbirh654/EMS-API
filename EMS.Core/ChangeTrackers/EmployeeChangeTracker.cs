using EMS.Core.Entities;

namespace EMS.Core.ChangeTrackers;

public static class EmployeeChangeTracker
{
    public static List<string> GetUpdatedFields(EmployeeDetails oldEmployee, Employee newEmployee)
    {
        List<string> updatedFields = new();

        if (oldEmployee.Name != newEmployee.Name)
            updatedFields.Add($"Name: '{oldEmployee.Name}' -> '{newEmployee.Name}'");
        if (oldEmployee.Email != newEmployee.Email)
            updatedFields.Add($"Email: '{oldEmployee.Email}' -> '{newEmployee.Email}'");
        if (oldEmployee.PhoneNumber != newEmployee.PhoneNumber)
            updatedFields.Add($"Phone: '{oldEmployee.PhoneNumber}' -> '{newEmployee.PhoneNumber}'");
        if (oldEmployee.BirthDate != newEmployee.BirthDate)
            updatedFields.Add($"DOB: '{oldEmployee.BirthDate}' -> '{newEmployee.BirthDate}'");
        if (oldEmployee.Address != newEmployee.Address)
            updatedFields.Add($"Address: '{oldEmployee.Address}' -> '{newEmployee.Address}'");

        return updatedFields;
    }
}
