using System;
using System.Collections.Generic;
using Scrap.Core;

namespace Scrap.Domain.Repositories
{
    /// <summary>
    /// Базовый обобщённый репозитарий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : BaseRepository
        where T : PersistentObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        protected BaseRepository(IModelContext context)
            : base(context)
        {
        }

        public abstract void Create(T data);

        public abstract IEnumerable<T> GetAll();

        public abstract T GetById(Guid id);

        public abstract void Update(T data);

        /// <summary>
        /// Удаление сущности по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public abstract bool Delete(Guid id);

        public virtual bool Delete(T data)
        {
            return Delete(data.Id);
        }

    }
}