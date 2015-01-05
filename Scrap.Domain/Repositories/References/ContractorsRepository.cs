using System.Collections.Generic;
using Scrap.Core;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;

namespace Scrap.Domain.Repositories.References
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