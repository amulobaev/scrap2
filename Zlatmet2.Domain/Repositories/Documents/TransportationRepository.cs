using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Domain.Entities.Documents;

namespace Zlatmet2.Domain.Repositories.Documents
{
    /// <summary>
    /// Репозитарий документов "Перевозка"
    /// </summary>
    public sealed class TransportationRepository : BaseRepository<Transportation>
    {
        static TransportationRepository()
        {
            // Создание маппингов

            // Информация о документе
            Mapper.CreateMap<Transportation, TransportationEntity>()
                .AfterMap((model, entity) =>
                {
                    foreach (TransportationItemEntity item in entity.Items)
                        item.DocumentId = model.Id;
                });
            Mapper.CreateMap<TransportationEntity, Transportation>()
                .ConstructUsing(x => new Transportation(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Табличная часть
            Mapper.CreateMap<TransportationItem, TransportationItemEntity>();
            Mapper.CreateMap<TransportationItemEntity, TransportationItem>()
                .ConstructUsing(x => new TransportationItem(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public TransportationRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Transportation data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportationEntity entity = Mapper.Map<Transportation, TransportationEntity>(data);
                context.DocumentTransportation.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Transportation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Transportation GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportationEntity entity = context.DocumentTransportation.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    var document = Mapper.Map<TransportationEntity, Transportation>(entity);
                    return document;
                }
                else
                    return null;
            }
        }

        public override void Update(Transportation data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportationEntity entity = context.DocumentTransportation.FirstOrDefault(x => x.Id == data.Id);
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