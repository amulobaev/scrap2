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
    public class RemainsRepository : BaseRepository<Remains>
    {
        static RemainsRepository()
        {
            // Создание маппингов

            // Информация о документе
            Mapper.CreateMap<Remains, RemainsEntity>()
                .ForMember(x => x.Items, opt => opt.Ignore());
            Mapper.CreateMap<RemainsEntity, Remains>()
                .ConstructUsing(x => new Remains(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<RemainsItem, RemainsItemEntity>();
            Mapper.CreateMap<RemainsItemEntity, RemainsItem>()
                .ConstructUsing(x => new RemainsItem(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public RemainsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Remains data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                RemainsEntity entity = Mapper.Map<Remains, RemainsEntity>(data);
                entity.Items = Mapper.Map(data.Items, entity.Items);
                context.DocumentRemains.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Remains> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Remains GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                RemainsEntity entity = context.DocumentRemains.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<RemainsEntity, Remains>(entity) : null;
            }
        }

        public override void Update(Remains data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                RemainsEntity entity = context.DocumentRemains.FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(data, entity);

                    // Новые и изменённые строки табличной части
                    foreach (RemainsItem item in data.Items)
                    {
                        // Новая строка
                        if (entity.Items.All(x => x.Id != item.Id))
                        {
                            entity.Items.Add(Mapper.Map<RemainsItem, RemainsItemEntity>(item));
                            continue;
                        }

                        // Существующая строка
                        RemainsItemEntity itemEntity = entity.Items.FirstOrDefault(x => x.Id == item.Id);
                        if (itemEntity != null)
                        {
                            Mapper.Map(item, itemEntity);
                            continue;
                        }
                    }

                    // Удалённые строки табличной части
                    for (int i = 0; i < entity.Items.Count; i++)
                    {
                        RemainsItemEntity itemEntity = entity.Items.ToList()[i];
                        if (data.Items.All(x => x.Id != itemEntity.Id))
                        {
                            RemainsItemEntity entityToRemove =
                                context.DocumentRemainsItems.FirstOrDefault(x => x.Id == itemEntity.Id);
                            if (entityToRemove != null)
                                context.DocumentRemainsItems.Remove(entityToRemove);
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
                RemainsEntity entity = context.DocumentRemains.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.DocumentRemains.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}