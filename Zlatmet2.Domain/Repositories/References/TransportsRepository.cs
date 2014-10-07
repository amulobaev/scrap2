using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Domain.Entities.References;

namespace Zlatmet2.Domain.Repositories.References
{
    /// <summary>
    /// Репозитарий справочника "Транспорт"
    /// </summary>
    public sealed class TransportsRepository : BaseRepository<Transport>
    {
        static TransportsRepository()
        {
            Mapper.CreateMap<Transport, TransportEntity>();
            Mapper.CreateMap<TransportEntity, Transport>()
                .ConstructUsing(x => new Transport(x.Id));
        }

        public TransportsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Transport data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportEntity entity = Mapper.Map<Transport, TransportEntity>(data);
                context.Transports.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Transport> GetAll()
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                List<TransportEntity> entities = context.Transports.ToList();
                return Mapper.Map<List<TransportEntity>, List<Transport>>(entities);
            }
        }

        public override Transport GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportEntity entity = context.Transports.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<TransportEntity, Transport>(entity) : null;
            }
        }

        public override void Update(Transport data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportEntity entity = context.Transports.FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(data, entity);
                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TransportEntity entity = context.Transports.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.Transports.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

    }
}