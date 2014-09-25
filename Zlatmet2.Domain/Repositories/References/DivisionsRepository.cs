using System;
using System.Collections.Generic;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Domain.Entities.References;

namespace Zlatmet2.Domain.Repositories.References
{
    /// <summary>
    /// Репозитарий справочника "Подразделения"
    /// </summary>
    public class DivisionsRepository : BaseRepository<Division>
    {
        static DivisionsRepository()
        {
        }

        public DivisionsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Division data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                DivisionEntity entity = Mapper.Map<Division, DivisionEntity>(data);
                context.Divisions.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Division> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Division GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Division data)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}