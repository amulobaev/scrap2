using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Scrap.Core;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;
using Scrap.Domain.Entities.References;

namespace Scrap.Domain.Repositories.References
{
    /// <summary>
    /// Базовый репозитарий для подрядчиков/заказчиков/баз
    /// </summary>
    public abstract class OrganizationsRepository : BaseRepository<Organization>
    {
        static OrganizationsRepository()
        {
            // Организации
            Mapper.CreateMap<Organization, OrganizationEntity>()
                .ForMember(x => x.Divisions, opt => opt.Ignore());

            Mapper.CreateMap<OrganizationEntity, Organization>()
                .ConstructUsing(x => new Organization(x.Id, (OrganizationType)x.Type))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Type, opt => opt.Ignore());

            // Подразделения
            Mapper.CreateMap<Division, DivisionEntity>();
            Mapper.CreateMap<DivisionEntity, Division>()
                .ConstructUsing(x => new Division(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        protected OrganizationsRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Organization data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity entity = Mapper.Map<Organization, OrganizationEntity>(data);
                entity.Divisions = Mapper.Map(data.Divisions, entity.Divisions);
                context.Organizations.Add(entity);
                context.SaveChanges();
            }
        }

        protected IEnumerable<Organization> GetAll(OrganizationType type)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity[] entities =
                    context.Organizations.Where(x => x.Type == (int)type).Include(x => x.Divisions).ToArray();
                Organization[] organizations = Mapper.Map<OrganizationEntity[], Organization[]>(entities);
                return organizations;
            }
        }

        public override Organization GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity entity =
                    context.Organizations.Include(x => x.Divisions).FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<OrganizationEntity, Organization>(entity) : null;
            }
        }

        public override void Update(Organization data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity entity =
                    context.Organizations.Include(x => x.Divisions).FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(data, entity);

                    // Новые и измененные подразделения
                    foreach (Division division in data.Divisions)
                    {
                        // Новое подразделение
                        if (entity.Divisions.All(x => x.Id != division.Id))
                        {
                            entity.Divisions.Add(Mapper.Map<Division, DivisionEntity>(division));
                            continue;
                        }

                        // Существующее подразделение
                        DivisionEntity divisionEntity = entity.Divisions.FirstOrDefault(x => x.Id == division.Id);
                        if (divisionEntity != null)
                        {
                            Mapper.Map(division, divisionEntity);
                            continue;
                        }
                    }

                    // Удалённые подразделения
                    for (int i = 0; i < entity.Divisions.Count; i++)
                    {
                        DivisionEntity divisionEntity = entity.Divisions.ToList()[i];
                        if (data.Divisions.All(x => x.Id != divisionEntity.Id))
                        {
                            DivisionEntity entityToRemove =
                                context.Divisions.FirstOrDefault(x => x.Id == divisionEntity.Id);
                            if (entityToRemove != null)
                                context.Divisions.Remove(entityToRemove);
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
                OrganizationEntity entity = context.Organizations.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.Organizations.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}