using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.References
{
    public class NomenclatureRepository : BaseRepository<Nomenclature>
    {
        static NomenclatureRepository()
        {
            Mapper.CreateMap<Nomenclature, NomenclatureDto>();
            Mapper.CreateMap<NomenclatureDto, Nomenclature>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public NomenclatureRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Nomenclature data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                NomenclatureDto dto = Mapper.Map<Nomenclature, NomenclatureDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        public override IEnumerable<Nomenclature> GetAll()
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query = QueryObject.GetAllQuery(typeof(NomenclatureDto));
                IEnumerable<NomenclatureDto> dtos = connection.Query<NomenclatureDto>(query);

                List<Nomenclature> nomenclatures = new List<Nomenclature>();
                foreach (NomenclatureDto dto in dtos)
                {
                    Nomenclature nomenclature = new Nomenclature(dto.Id);
                    Mapper.Map(dto, nomenclature);
                    nomenclatures.Add(nomenclature);
                }
                return nomenclatures;
            }
        }

        public override Nomenclature GetById(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query = QueryObject.GetByIdQuery(typeof(Nomenclature));
                var dto = connection.Query<NomenclatureDto>(query, new { Id = id }).FirstOrDefault();
                return dto != null ? Mapper.Map<NomenclatureDto, Nomenclature>(dto) : null;
            }
        }

        public override void Update(Nomenclature data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                NomenclatureDto dto = Mapper.Map<Nomenclature, NomenclatureDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);
            }
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}