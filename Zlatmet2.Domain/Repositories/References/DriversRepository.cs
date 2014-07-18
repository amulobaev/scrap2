using System.Collections.Generic;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Repositories.References
{
    public class DriversRepository : EmployeesRepository
    {
        public DriversRepository(IModelContext context)
            : base(context)
        {
        }

        public override IEnumerable<Employee> GetAll()
        {
            return GetAll(EmployeeType.Driver);
        }
    }
}