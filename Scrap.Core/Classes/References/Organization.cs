using System;
using System.Collections.Generic;
using Scrap.Core.Enums;

namespace Scrap.Core.Classes.References
{
    public class Organization : PersistentObject
    {
        private readonly OrganizationType _type;
        private List<Division> _divisions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public Organization(Guid id, OrganizationType type)
            : base(id)
        {
            _type = type;
        }

        public OrganizationType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        public string Inn { get; set; }

        public string Bik { get; set; }

        public string Bank { get; set; }

        public string Contract { get; set; }

        public List<Division> Divisions
        {
            get { return _divisions ?? (_divisions = new List<Division>()); }
        }

    }
}