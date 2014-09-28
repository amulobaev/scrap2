using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Domain.Entities;

namespace Zlatmet2.Domain.Repositories.Service
{
    public class TemplatesRepository : BaseRepository<Template>
    {
        static TemplatesRepository()
        {
            Mapper.CreateMap<Template, TemplateEntity>();
            Mapper.CreateMap<TemplateEntity, Template>()
                .ConstructUsing(x => new Template(x.Id))
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
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity entity = Mapper.Map<Template, TemplateEntity>(data);
                context.Templates.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Template> GetAll()
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity[] entities = context.Templates.ToArray();
                return Mapper.Map<TemplateEntity[], Template[]>(entities);
            }
        }

        public override Template GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity entity = context.Templates.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<TemplateEntity, Template>(entity) : null;
            }
        }

        public Template GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity entity = context.Templates.FirstOrDefault(x => x.Name == name);
                return entity != null ? Mapper.Map<TemplateEntity, Template>(entity) : null;
            }
        }

        public override void Update(Template data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity entity = context.Templates.FirstOrDefault(x => x.Id == data.Id);
                if (entity != null)
                {
                    Mapper.Map(entity, data);
                    context.SaveChanges();
                }
            }
        }

        public override bool Delete(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                TemplateEntity entity = context.Templates.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.Templates.Remove(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

    }
}