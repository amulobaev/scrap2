﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain.Entities.References;

namespace Zlatmet2.Domain.Repositories.References
{
    /// <summary>
    /// Базовый репозитарий для подрядчиков/заказчиков/баз
    /// </summary>
    public abstract class OrganizationsRepository : BaseRepository<Organization>
    {
        private readonly DivisionsRepository _divisionsRepository;

        static OrganizationsRepository()
        {
            Mapper.CreateMap<Division, DivisionEntity>();
            Mapper.CreateMap<DivisionEntity, Division>()
                .ConstructUsing(x => new Division(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<Organization, OrganizationEntity>();
            Mapper.CreateMap<OrganizationEntity, Organization>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Type, opt => opt.Ignore());
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        protected OrganizationsRepository(IModelContext context)
            : base(context)
        {
            _divisionsRepository = new DivisionsRepository(context);
        }

        /// <summary>
        /// Репозитарий для подразделений
        /// </summary>
        private DivisionsRepository DivisionsRepository
        {
            get { return _divisionsRepository; }
        }

        public override void Create(Organization data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity organizationEntity = Mapper.Map<Organization, OrganizationEntity>(data);
                context.Organizations.Add(organizationEntity);

                context.SaveChanges();
            }
        }

        protected IEnumerable<Organization> GetAll(OrganizationType type)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity[] entities = context.Organizations.Where(x => x.Type == (int)type).ToArray();
                return Mapper.Map<OrganizationEntity[], Organization[]>(entities);
            }
        }

        public override Organization GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Organization data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                OrganizationEntity entity = context.Organizations.FirstOrDefault(x => x.Id == data.Id);
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