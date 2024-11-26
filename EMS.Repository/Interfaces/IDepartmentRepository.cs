using EMS.Core.Entities;

namespace EMS.Repository.Interfaces;

public interface IDepartmentRepository : IGenericGetRepository<Department>, IGenericPostRepository<Department>
{
}
