using System.Collections.Generic;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Domain.Repositories.References
{
    public sealed class BasesRepository : OrganizationsRepository
    {
        public BasesRepository(IModelContext context)
            : base(context)
        {
        }

        public override IEnumerable<Organization> GetAll()
        {
            return GetAll(OrganizationType.Base);
        }

    }
}
