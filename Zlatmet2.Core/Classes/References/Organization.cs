using System;
using System.Collections.Generic;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Core.Classes.References
{
    public class Organization : PersistentObject
    {
        private List<Division> _divisions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        public Organization(Guid id)
            : base(id)
        {
        }

        public OrganizationType Type { get; set; }

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