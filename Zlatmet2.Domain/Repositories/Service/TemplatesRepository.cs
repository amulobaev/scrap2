using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Domain.Dto.Service;
using Zlatmet2.Domain.Tools;

namespace Zlatmet2.Domain.Repositories.Service
{
    public class TemplatesRepository : BaseRepository<Template>
    {
        static TemplatesRepository()
        {
            Mapper.CreateMap<Template, TemplateDto>();
            Mapper.CreateMap<TemplateDto, Template>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public TemplatesRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Template data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                TemplateDto dto = Mapper.Map<Template, TemplateDto>(data);
                connection.Execute(dto.InsertQuery(), dto);
            }
        }

        public override IEnumerable<Template> GetAll()
        {
            using (var connection = ConnectionFactory.Create())
            {
                IEnumerable<TemplateDto> dtos =
                    connection.Query<TemplateDto>(QueryObject.GetAllQuery(typeof(TemplateDto)));

                var templates = new List<Template>();
                foreach (TemplateDto dto in dtos)
                {
                    Template template = new Template(dto.Id);
                    Mapper.Map(dto, template);
                    templates.Add(template);
                }
                return templates;
            }
        }

        public override Template GetById(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                TemplateDto dto =
                    connection.Query<TemplateDto>(QueryObject.GetByIdQuery(typeof(TemplateDto)), new { Id = id })
                        .FirstOrDefault();
                if (dto != null)
                {
                    Template template = new Template(dto.Id);
                    Mapper.Map(dto, template);
                    return template;
                }
                else
                    return null;
            }
        }

        public Template GetByName(string name)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query = string.Format("SELECT * FROM {0} WHERE Name = @Name",
                    QueryObject.GetTable(typeof(TemplateDto)));
                TemplateDto dto = connection.Query<TemplateDto>(query, new { Name = name }).FirstOrDefault();
                if (dto != null)
                {
                    Template template = new Template(dto.Id);
                    Mapper.Map(dto, template);
                    return template;
                }
                else
                    return null;
            }
        }

        public override void Update(Template data)
        {
            using (var connection = ConnectionFactory.Create())
            {
                TemplateDto dto = Mapper.Map<Template, TemplateDto>(data);
                connection.Execute(dto.UpdateQuery(), dto);
            }
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}