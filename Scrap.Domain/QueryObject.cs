using System;
using System.Collections.Generic;
using System.Linq;
using Scrap.Domain.Dto;

namespace Scrap.Domain
{
    public static class QueryObject
    {
        #region Поля

        private static Dictionary<Type, string> _tables;

        private static Dictionary<Type, IEnumerable<string>> _propertyNames;

        private static Dictionary<Type, string> _createQueries;

        private static Dictionary<Type, string> _getAllQueries;

        private static Dictionary<Type, string> _getByIdQueries;

        private static Dictionary<Type, string> _updateQueries;

        private static Dictionary<Type, string> _deleteQueries;

        #endregion

        #region Свойства

        /// <summary>
        /// Имена таблиц сопоставленных с DTO
        /// </summary>
        private static Dictionary<Type, string> Tables
        {
            get { return _tables ?? (_tables = new Dictionary<Type, string>()); }
        }

        /// <summary>
        /// Типы DTO и наименования свойств
        /// </summary>
        private static Dictionary<Type, IEnumerable<string>> PropertyNames
        {
            get { return _propertyNames ?? (_propertyNames = new Dictionary<Type, IEnumerable<string>>()); }
        }

        /// <summary>
        /// Запросы INSERT ...
        /// </summary>
        private static Dictionary<Type, string> CreateQueries
        {
            get { return _createQueries ?? (_createQueries = new Dictionary<Type, string>()); }
        }

        private static Dictionary<Type, string> GetAllQueries
        {
            get { return _getAllQueries ?? (_getAllQueries = new Dictionary<Type, string>()); }
        }

        private static Dictionary<Type, string> GetByIdQueries
        {
            get { return _getByIdQueries ?? (_getByIdQueries = new Dictionary<Type, string>()); }
        }

        private static Dictionary<Type, string> UpdateQueries
        {
            get { return _updateQueries ?? (_updateQueries = new Dictionary<Type, string>()); }
        }

        private static Dictionary<Type, string> DeleteQueries
        {
            get { return _deleteQueries ?? (_deleteQueries = new Dictionary<Type, string>()); }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        static QueryObject()
        {
        }

        #region Методы

        public static string CreateQuery(BaseDto dto)
        {
            return CreateQuery(dto.GetType());
        }

        public static string CreateQuery(Type dtoType)
        {
            if (CreateQueries.ContainsKey(dtoType))
                return CreateQueries[dtoType];
            else
            {
                string table = GetTable(dtoType);
                List<string> propertyNames = GetPropertiesNames(dtoType).ToList();
                return
                    CreateQueries[dtoType] =
                        string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", table,
                            string.Join(",", propertyNames.Select(x => "[" + x + "]")),
                            string.Join(",", propertyNames.Select(x => "@" + x)));
            }
        }

        public static string GetAllQuery(BaseDto dto)
        {
            return GetAllQuery(dto.GetType());
        }

        public static string GetAllQuery(Type dtoType)
        {
            if (GetAllQueries.ContainsKey(dtoType))
                return GetAllQueries[dtoType];
            else
            {
                string table = GetTable(dtoType);
                return GetAllQueries[dtoType] = string.Format("SELECT * FROM [{0}]", table);
            }
        }

        public static string GetByIdQuery(BaseDto dto)
        {
            return GetByIdQuery(dto.GetType());
        }

        public static string GetByIdQuery(Type dtoType)
        {
            if (GetByIdQueries.ContainsKey(dtoType))
                return GetByIdQueries[dtoType];
            else
            {
                string table = GetTable(dtoType);
                return GetByIdQueries[dtoType] = string.Format("SELECT * FROM [{0}] WHERE Id = @Id", table);
            }
        }

        public static string UpdateQuery(BaseDto dto)
        {
            return UpdateQuery(dto.GetType());
        }

        public static string UpdateQuery(Type dtoType)
        {
            if (UpdateQueries.ContainsKey(dtoType))
                return UpdateQueries[dtoType];
            else
            {
                string table = GetTable(dtoType);
                List<string> propertyNames = GetPropertiesNames(dtoType).ToList();
                return
                    UpdateQueries[dtoType] =
                        string.Format("UPDATE [{0}] SET {1} WHERE Id = @Id", table,
                            string.Join(",", propertyNames.Where(x => x != "Id").Select(x => "[" + x + "] = @" + x)));
            }
        }

        public static string DeleteQuery(BaseDto dto)
        {
            return DeleteQuery(dto.GetType());
        }

        public static string DeleteQuery(Type dtoType)
        {
            if (DeleteQueries.ContainsKey(dtoType))
                return DeleteQueries[dtoType];
            else
            {
                string table = GetTable(dtoType);
                return DeleteQueries[dtoType] = string.Format("DELETE FROM [{0}] WHERE Id = @Id", table);
            }
        }

        public static string GetTable(BaseDto dto)
        {
            return GetTable(dto.GetType());
        }

        /// <summary>
        /// Таблица для DTO
        /// </summary>
        /// <param name="dtoType"></param>
        /// <returns></returns>
        public static string GetTable(Type dtoType)
        {
            if (Tables.ContainsKey(dtoType))
                return Tables[dtoType];
            else
            {
                var tableAttribute =
                    dtoType.GetCustomAttributes(typeof(TableAttribute), false)
                        .OfType<TableAttribute>()
                        .FirstOrDefault();
                if (tableAttribute == null || string.IsNullOrEmpty(tableAttribute.Name))
                    throw new Exception("Для DTO не указана таблица");
                return Tables[dtoType] = tableAttribute.Name;
            }
        }

        /// <summary>
        /// Список свойств типа
        /// </summary>
        /// <param name="dtoType"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetPropertiesNames(Type dtoType)
        {
            if (PropertyNames.ContainsKey(dtoType))
                return PropertyNames[dtoType];
            else
            {
                var properties = dtoType.GetProperties();
                var filteredProperties =
                    properties.Where(
                        p =>
                            !p.PropertyType.IsGenericType ||
                            (p.PropertyType.IsGenericType && p.PropertyType.IsValueType));
                return PropertyNames[dtoType] = filteredProperties.Select(x => x.Name).ToList();
            }
        }

        #endregion

    }
}
