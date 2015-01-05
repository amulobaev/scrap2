using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Scrap.Core;
using Scrap.Core.Classes.References;
using Scrap.Domain.Entities.References;

namespace Scrap.Domain.Repositories.References
{
    /// <summary>
    /// Репозитарий справочника "Номенклатура"
    /// </summary>
    public sealed class NomenclatureRepository : BaseRepository<Nomenclature>
    {
        static NomenclatureRepository()
        {
            Mapper.CreateMap<Nomenclature, NomenclatureEntity>();
            Mapper.CreateMap<NomenclatureEntity, Nomenclature>()
                .ConstructUsing(x => new Nomenclature(x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        public NomenclatureRepository(IModelContext context)
            : base(context)
        {
        }

        public override void Create(Nomenclature data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                NomenclatureEntity entity = Mapper.Map<Nomenclature, NomenclatureEntity>(data);
                context.Nomenclatures.Add(entity);
                context.SaveChanges();
            }
        }

        public override IEnumerable<Nomenclature> GetAll()
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                NomenclatureEntity[] entities = context.Nomenclatures.ToArray();
                return Mapper.Map<NomenclatureEntity[], Nomenclature[]>(entities);
            }
        }

        public override Nomenclature GetById(Guid id)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                NomenclatureEntity entity = context.Nomenclatures.FirstOrDefault(x => x.Id == id);
                return entity != null ? Mapper.Map<NomenclatureEntity, Nomenclature>(entity) : null;
            }
        }

        public override void Update(Nomenclature data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (ZlatmetContext context = new ZlatmetContext())
            {
                NomenclatureEntity entity = context.Nomenclatures.FirstOrDefault(x => x.Id == data.Id);
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
                NomenclatureEntity entity = context.Nomenclatures.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    context.Nomenclatures.Remove(entity);
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