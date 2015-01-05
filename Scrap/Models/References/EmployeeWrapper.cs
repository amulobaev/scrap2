using System;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;

namespace Scrap.Models.References
{
    /// <summary>
    /// Обертка для класса сотрудника
    /// </summary>
    public class EmployeeWrapper : BaseReferenceWrapper<Employee>
    {
        private string _name;
        private string _fullName;
        private string _phone;

        public EmployeeWrapper(EmployeeType employeeType, string name)
        {
            Id = Guid.NewGuid();
            EmployeeType = employeeType;
            Name = name;
        }

        public EmployeeWrapper(Employee employee)
            : base(employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            Id = employee.Id;
            _name = employee.Name;
            _fullName = employee.FullName;
            _phone = employee.Phone;
        }

        public EmployeeType EmployeeType { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (value == _fullName)
                    return;
                _fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone)
                    return;
                _phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Employee(Id, EmployeeType);
            Container.Name = Name;
            Container.FullName = FullName;
            Container.Phone = Phone;
        }

    }
}