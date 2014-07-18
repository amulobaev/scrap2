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
    public class ProcessingRepository : BaseRepository<Processing, ProcessingDto>
    {
        static ProcessingRepository()
        {
            // Шапка
            Mapper.CreateMap<Processing, ProcessingDto>()
                .AfterMap((m, d) =>
                {
                    foreach (var dto in d.Items)
                        dto.DocumentId = m.Id;
                });
            Mapper.CreateMap<ProcessingDto, Processing>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<ProcessingItem, ProcessingItemDto>();
            Mapper.CreateMap<ProcessingItemDto, ProcessingItem>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public ProcessingRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Processing data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                ProcessingDto dto = Mapper.Map<Processing, ProcessingDto>(data);
                connection.Execute(dto.InsertQuery(), dto);

                // Сохранение табличной части
                connection.Execute(QueryObject.CreateQuery(typeof(ProcessingItemDto)), dto.Items);
            }
        }

        public override IEnumerable<Processing> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Processing GetById(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query;

                // Шапка
                query = QueryObject.GetByIdQuery(typeof(ProcessingDto));
                var dto = connection.Query<ProcessingDto>(query, new { Id = id }).FirstOrDefault();
                if (dto == null)
                    return null;

                var document = new Processing(id);
                Mapper.Map(dto, document);

                // Табличная часть
                query = string.Format("SELECT * FROM [{0}] WHERE DocumentId = @Id",
                    QueryObject.GetTable(typeof(ProcessingItemDto)));
                var items = connection.Query<ProcessingItemDto>(query, new { Id = id }).ToList();
                foreach (var itemDto in items)
                {
                    ProcessingItem item = new ProcessingItem(itemDto.Id);
                    Mapper.Map(itemDto, item);
                    document.Items.Add(item);
                }

                return document;
            }
        }

        public override void Update(Processing data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                // Шапка
                ProcessingDto dto = Mapper.Map<Processing, ProcessingDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);

                // Табличная часть
                connection.Execute(
                    string.Format("DELETE FROM [{0}] WHERE DocumentId = @Id",
                        QueryObject.GetTable(typeof(ProcessingItemDto))), new { dto.Id });
                connection.Execute(QueryObject.CreateQuery(typeof(ProcessingItemDto)), dto.Items);
            }
        }

    }
}