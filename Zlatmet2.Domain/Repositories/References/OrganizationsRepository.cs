using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Domain.Dto.References;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.References
{
    /// <summary>
    /// Базовый репозитарий для подрядчиков/заказчиков/баз
    /// </summary>
    public abstract class OrganizationsRepository : BaseRepository<Organization, OrganizationDto>
    {
        private readonly DivisionsRepository _divisionsRepository;

        static OrganizationsRepository()
        {
            Mapper.CreateMap<Organization, OrganizationDto>();
            Mapper.CreateMap<OrganizationDto, Organization>()
                .ForMember(x => x.Id, opt => opt.Ignore());
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
            using (var connection = ConnectionFactory.Create())
            {
                OrganizationDto dto = Mapper.Map<Organization, OrganizationDto>(data);
                connection.Execute(dto.InsertQuery(), dto);

                DivisionsRepository.Create(dto.Divisions);
            }
        }

        protected IEnumerable<Organization> GetAll(OrganizationType type)
        {
            using (var connection = ConnectionFactory.Create())
            {
                List<OrganizationDto> dtos =
                    connection.Query<OrganizationDto>("SELECT * FROM [ReferenceOrganizations] WHERE Type = @Type",
                        new { Type = (int)type }).ToList();
                foreach (OrganizationDto organizationDto in dtos)
                {
                    List<DivisionDto> divisions =
                        connection.Query<DivisionDto>("SELECT * FROM [ReferenceDivisions] WHERE OrganizationId = @Id",
                            new { organizationDto.Id }).ToList();
                    if (divisions.Any())
                        organizationDto.Divisions.AddRange(divisions);
                }

                List<Organization> organizations = new List<Organization>();
                foreach (OrganizationDto organizationDto in dtos)
                {
                    Organization organization = new Organization(organizationDto.Id);
                    Mapper.Map(organizationDto, organization);
                    organizations.Add(organization);
                }
                return organizations;
            }
        }

        public override Organization GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Organization data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                OrganizationDto dto = Mapper.Map<Organization, OrganizationDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);

                // Подразделения
                //connection.Execute("DELETE FROM [ReferenceDivisions] WHERE OrganizationId = @Id", new { dto.Id });
                //InsertDivisions(connection, dto.Divisions);
            }
        }

    }
}