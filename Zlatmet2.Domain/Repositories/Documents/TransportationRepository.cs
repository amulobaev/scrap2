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
    /// <summary>
    /// Репозитарий документов "Перевозка"
    /// </summary>
    public sealed class TransportationRepository : BaseRepository<Transportation, TransportationDto>
    {
        static TransportationRepository()
        {
            // Создание маппингов
            // Информация о документе
            Mapper.CreateMap<Transportation, TransportationDto>()
                .AfterMap((m, d) =>
                {
                    foreach (var dto in d.Items)
                        dto.DocumentId = m.Id;
                });
            Mapper.CreateMap<TransportationDto, Transportation>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<TransportationItem, TransportationItemDto>();
            Mapper.CreateMap<TransportationItemDto, TransportationItem>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public TransportationRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Transportation data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                TransportationDto dto = Mapper.Map<Transportation, TransportationDto>(data);
                connection.Execute(dto.InsertQuery(), dto);

                // Сохранение табличной части
                connection.Execute(QueryObject.CreateQuery(typeof(TransportationItemDto)), dto.Items);
            }
        }

        public override IEnumerable<Transportation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Transportation GetById(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query;

                // Шапка
                query = QueryObject.GetByIdQuery(typeof(TransportationDto));
                var dto = connection.Query<TransportationDto>(query, new { Id = id }).FirstOrDefault();
                if (dto == null)
                    return null;

                var document = new Transportation(id);
                Mapper.Map(dto, document);

                // Табличная часть
                query = string.Format("SELECT * FROM [{0}] WHERE DocumentId = @Id",
                    QueryObject.GetTable(typeof(TransportationItemDto)));
                var items = connection.Query<TransportationItemDto>(query, new { Id = id }).ToList();
                foreach (var itemDto in items)
                {
                    TransportationItem item = new TransportationItem(itemDto.Id);
                    Mapper.Map(itemDto, item);
                    document.Items.Add(item);
                }

                return document;
            }
        }

        public override void Update(Transportation data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                TransportationDto dto = Mapper.Map<Transportation, TransportationDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);

                // Табличная часть
                connection.Execute(
                    string.Format("DELETE FROM [{0}] WHERE DocumentId = @Id",
                        QueryObject.GetTable(typeof(TransportationItemDto))), new { dto.Id });
                connection.Execute(QueryObject.CreateQuery(typeof(TransportationItemDto)), dto.Items);
            }
        }

    }
}