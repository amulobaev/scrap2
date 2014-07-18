using System;
using System.Collections.Generic;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.Domain.Repositories
{
    /// <summary>
    /// Базовый обобщённый репозитарий
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public abstract class BaseRepository<TModel, TDto> : BaseRepository
        where TModel : PersistentObject
        where TDto : BaseDto
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        protected BaseRepository(IModelContext context)
            : base(context)
        {
        }

        public abstract void Create(TModel data);

        public abstract IEnumerable<TModel> GetAll();

        public abstract TModel GetById(Guid id);

        public abstract void Update(TModel data);

        public virtual bool Delete(TModel data)
        {
            return Delete(data.Id);
        }

        public virtual bool Delete(Guid id)
        {
            using (var connection = ConnectionFactory.Create())
            {
                string query = QueryObject.DeleteQuery(typeof(TDto));
                return connection.Execute(query, new { Id = id }) > 0;
            }
        }

    }
}