using System;
using System.Collections.Generic;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.References
{
    public class DivisionsRepository : BaseRepository<Division>
    {
        static DivisionsRepository()
        {
            Mapper.CreateMap<Division, DivisionDto>();
            Mapper.CreateMap<DivisionDto, Division>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public DivisionsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Division data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                DivisionDto dto = Mapper.Map<Division, DivisionDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        public void Create(IEnumerable<DivisionDto> divisions)
        {
            using (var connection = ConnectionFactory.Create())
            {
                connection.Execute(QueryObject.CreateQuery(typeof(DivisionDto)), divisions);
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