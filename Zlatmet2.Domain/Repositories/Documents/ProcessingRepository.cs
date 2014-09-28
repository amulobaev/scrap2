using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Domain.Entities.Documents;

namespace Zlatmet2.Domain.Repositories.Documents
{
    public class ProcessingRepository : BaseRepository<Processing>
    {
        static ProcessingRepository()
        {
            // Создание маппингов

            // Информация о документе
            Mapper.CreateMap<Processing, ProcessingEntity>()
                .AfterMap((model, entity) =>
                {
                    foreach (ProcessingItemEntity item in entity.Items)
                        item.DocumentId = model.Id;
                });
            Mapper.CreateMap<ProcessingEntity, Processing>()
                .ConstructUsing(x => new Processing(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<ProcessingItem, ProcessingItemEntity>();
            Mapper.CreateMap<ProcessingItemEntity, ProcessingItem>()
                .ConstructUsing(x => new ProcessingItem(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public ProcessingRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Processing data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                ProcessingEntity entity = Mapper.Map<Processing, ProcessingEntity>(data);
                context.DocumentProcessing.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Processing> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Processing GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                ProcessingEntity entity = context.DocumentProcessing.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<ProcessingEntity, Processing>(entity) : null;
            }
        }

        public override void Update(Processing data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                ProcessingEntity entity = context.DocumentProcessing.FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(data, entity);
                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}