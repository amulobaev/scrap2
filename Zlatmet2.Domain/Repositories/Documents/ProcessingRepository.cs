using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                .ForMember(x => x.Items, opt => opt.Ignore());
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
                entity.Items = Mapper.Map(data.Items, entity.Items);
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
                ProcessingEntity entity =
                    context.DocumentProcessing.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
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

                    // Новые и изменённые строки табличной части
                    foreach (ProcessingItem item in data.Items)
                    {
                        // Новая строка
                        if (entity.Items.All(x => x.Id != item.Id))
                        {
                            entity.Items.Add(Mapper.Map<ProcessingItem, ProcessingItemEntity>(item));
                            continue;
                        }

                        // Существующая строка
                        ProcessingItemEntity itemEntity = entity.Items.FirstOrDefault(x => x.Id == item.Id);
                        if (itemEntity != null)
                        {
                            Mapper.Map(item, itemEntity);
                            continue;
                        }
                    }

                    // Удалённые строки табличной части
                    for (int i = 0; i < entity.Items.Count; i++)
                    {
                        ProcessingItemEntity itemEntity = entity.Items.ToList()[i];
                        if (data.Items.All(x => x.Id != itemEntity.Id))
                        {
                            ProcessingItemEntity entityToRemove =
                                context.DocumentProcessingItems.FirstOrDefault(x => x.Id == itemEntity.Id);
                            if (entityToRemove != null)
                                context.DocumentProcessingItems.Remove(entityToRemove);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                ProcessingEntity entity = context.DocumentProcessing.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.DocumentProcessing.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}