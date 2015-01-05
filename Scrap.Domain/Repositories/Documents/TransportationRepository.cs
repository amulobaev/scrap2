using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Scrap.Core;
using Scrap.Core.Classes.Documents;
using Scrap.Domain.Entities.Documents;

namespace Scrap.Domain.Repositories.Documents
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
                .ForMember(x => x.Items, opt => opt.Ignore());
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
                entity.Items = Mapper.Map(data.Items, entity.Items);
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
                TransportationEntity entity =
                    context.DocumentTransportation.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<TransportationEntity, Transportation>(entity) : null;
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

                    // Новые и изменённые строки табличной части
                    foreach (TransportationItem item in data.Items)
                    {
                        // Новая строка
                        if (entity.Items.All(x => x.Id != item.Id))
                        {
                            entity.Items.Add(Mapper.Map<TransportationItem, TransportationItemEntity>(item));
                            continue;
                        }

                        // Существующая строка
                        TransportationItemEntity itemEntity = entity.Items.FirstOrDefault(x => x.Id == item.Id);
                        if (itemEntity != null)
                        {
                            Mapper.Map(item, itemEntity);
                            continue;
                        }
                    }

                    // Удалённые строки табличной части
                    for (int i = 0; i < entity.Items.Count; i++)
                    {
                        TransportationItemEntity itemEntity = entity.Items.ToList()[i];
                        if (data.Items.All(x => x.Id != itemEntity.Id))
                        {
                            TransportationItemEntity entityToRemove =
                                context.DocumentTransportationItems.FirstOrDefault(x => x.Id == itemEntity.Id);
                            if (entityToRemove != null)
                                context.DocumentTransportationItems.Remove(entityToRemove);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (var context = new ZlatmetContext())
            {
                TransportationEntity entity = context.DocumentTransportation.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.DocumentTransportation.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Замена контрагента в документах
        /// </summary>
        /// <param name="contractorId"></param>
        /// <param name="divisionId"></param>
        /// <param name="newContractorId"></param>
        /// <param name="newDivisionId"></param>
        public void ConvertContractorToDivision(Guid contractorId, Guid divisionId, Guid newContractorId, Guid newDivisionId)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                List<TransportationEntity> documents = null;

                // Найти документы, у которых заказчик данный контрагент
                documents =
                    context.DocumentTransportation.Where(
                        x => x.SupplierId == contractorId && x.SupplierDivisionId == divisionId).ToList();
                foreach (TransportationEntity document in documents)
                {
                    document.SupplierId = newContractorId;
                    document.SupplierDivisionId = newDivisionId;
                }

                // Найти документы, у которых поставщик данный контрагент
                documents =
                    context.DocumentTransportation.Where(
                        x => x.CustomerId == contractorId && x.CustomerDivisionId == divisionId).ToList();
                foreach (TransportationEntity document in documents)
                {
                    document.CustomerId = newContractorId;
                    document.CustomerDivisionId = newDivisionId;
                }

                // Сохранить изменения
                context.SaveChanges();
            }
        }
    }
}