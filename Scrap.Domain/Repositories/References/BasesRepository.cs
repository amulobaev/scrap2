using System.Collections.Generic;
using Scrap.Core;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;

namespace Scrap.Domain.Repositories.References
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
