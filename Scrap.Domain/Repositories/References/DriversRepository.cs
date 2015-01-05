using System.Collections.Generic;
using Scrap.Core;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;

namespace Scrap.Domain.Repositories.References
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