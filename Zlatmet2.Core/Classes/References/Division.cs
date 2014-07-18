using System;

namespace Zlatmet2.Core.Classes.References
{
    /// <summary>
    /// Подразделение поставщика/заказчика
    /// </summary>
    public class Division : PersistentObject
    {
        private static Division _empty;

        public Division(Guid id)
            : base(id)
        {
        }

        public Guid OrganizationId { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public static Division Empty
        {
            get { return _empty ?? (_empty = new Division(Guid.Parse("{94383EB4-5144-4FF4-A239-C9986F535F52}"))); }
        }
    }
}
