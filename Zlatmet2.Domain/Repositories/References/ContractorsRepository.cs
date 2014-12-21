using System.Collections.Generic;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Repositories.References
{
    public class ContractorsRepository : OrganizationsRepository
    {
        public ContractorsRepository(IModelContext context)
            : base(context)
        {
        }

        public override IEnumerable<Organization> GetAll()
        {
            return GetAll(OrganizationType.Contractor);
        }

    }
}