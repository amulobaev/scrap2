using System;
using Zlatmet2.Core.Enums;

namespace Zlatmet2.Core.Classes.References
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee : PersistentObject
    {
        private readonly EmployeeType _type;
        private static Employee _empty;

        public Employee(Guid id, EmployeeType type)
            : base(id)
        {
            _type = type;
        }

        /// <summary>
        /// Тип
        /// </summary>
        public EmployeeType Type
        {
            get { return _type; }
        }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public static Employee Empty
        {
            get
            {
                return _empty ??
                       (_empty = new Employee(Guid.Parse("{65C18211-66F3-4502-9F0D-A064C875913B}"), EmployeeType.Driver));
            }
        }
    }
}