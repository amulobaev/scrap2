using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Domain.Dto.Documents;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.Documents
{
    public class RemainsRepository : BaseRepository<Remains>
    {
        static RemainsRepository()
        {
            // Шапка
            Mapper.CreateMap<Remains, RemainsDto>()
                .AfterMap((m, d) =>
                {
                    foreach (var dto in d.Items)
                        dto.DocumentId = m.Id;
                });
            Mapper.CreateMap<RemainsDto, Remains>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<RemainsItem, RemainsItemDto>();
            Mapper.CreateMap<RemainsItemDto, RemainsItem>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public RemainsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Remains data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                RemainsDto dto = Mapper.Map<Remains, RemainsDto>(data);
                connection.Execute(dto.InsertQuery(), dto);

                // Сохранение табличной части
                connection.Execute(QueryObject.CreateQuery(typeof(RemainsItemDto)), dto.Items);
            }
        }

        public override IEnumerable<Remains> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Remains GetById(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query;

                // Шапка
                query = QueryObject.GetByIdQuery(typeof(RemainsDto));
                var dto = connection.Query<RemainsDto>(query, new { Id = id }).FirstOrDefault();
                if (dto == null)
                    return null;

                var document = new Remains(id);
                Mapper.Map(dto, document);

                // Табличная часть
                query = string.Format("SELECT * FROM [{0}] WHERE DocumentId = @Id",
                    QueryObject.GetTable(typeof(RemainsItemDto)));
                var items = connection.Query<RemainsItemDto>(query, new { Id = id }).ToList();
                foreach (var itemDto in items)
                {
                    RemainsItem item = new RemainsItem(itemDto.Id);
                    Mapper.Map(itemDto, item);
                    document.Items.Add(item);
                }

                return document;
            }
        }

        public override void Update(Remains data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                RemainsDto dto = Mapper.Map<Remains, RemainsDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);

                // Табличная часть
                connection.Execute(
                    string.Format("DELETE FROM [{0}] WHERE DocumentId = @Id",
                        QueryObject.GetTable(typeof(RemainsItemDto))), new { dto.Id });
                connection.Execute(QueryObject.CreateQuery(typeof(RemainsItemDto)), dto.Items);
            }
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}