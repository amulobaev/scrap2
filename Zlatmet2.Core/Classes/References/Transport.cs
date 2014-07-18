using System;

namespace Zlatmet2.Core.Classes.References
{
    public class Transport : PersistentObject
    {
        private static Transport _empty;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        public Transport(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Марка авто
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер авто
        /// </summary>
        public string Number { get; set; }

        public double Tara { get; set; }

        /// <summary>
        /// Идентификатор водителя
        /// </summary>
        public Guid? DriverId { get; set; }

        //public string Driver { get; set; } // ФИО водителя

        public static Transport Empty
        {
            get { return _empty ?? (_empty = new Transport(Guid.Parse("{F934B29A-9E69-41FA-A13A-71AE0119E21C}"))); }
        }
    }
}