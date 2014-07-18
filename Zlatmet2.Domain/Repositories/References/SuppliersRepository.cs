using System.Collections.Generic;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Repositories.References
{
    public class SuppliersRepository : OrganizationsRepository
    {
        public SuppliersRepository(IModelContext context)
            : base(context)
        {
        }

        public override IEnumerable<Organization> GetAll()
        {
            return GetAll(OrganizationType.Supplier);
        }
    }
}