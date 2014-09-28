using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
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
                .AfterMap((model, entity) =>
                {
                    foreach (RemainsItemEntity item in entity.Items)
                        item.DocumentId = model.Id;
                });
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
                RemainsEntity entity = context.DocumentRemains.FirstOrDefault(x => x.Id == id);
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