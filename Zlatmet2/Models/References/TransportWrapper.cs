using System;
using System.Linq;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.Models.References
{
    public class TransportWrapper : BaseReferenceWrapper<Transport>
    {
        private string _name;
        private string _carNumber;
        private double _tara = 0;
        private Employee _driver;

        public TransportWrapper(Transport transport = null)
            : base(transport)
        {
            if (transport != null)
            {
                Id = transport.Id;
                _name = transport.Name;
                _carNumber = transport.Number;
                _tara = transport.Tara;
                _driver = transport.DriverId.HasValue
                    ? MainStorage.Instance.Drivers.FirstOrDefault(x => x.Id == transport.DriverId.Value)
                    : null;
            }
            else
            {
                Id = Guid.NewGuid();
                Name = "Новый транспорт";
            }
        }

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

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                if (value == _carNumber)
                    return;
                _carNumber = value;
                RaisePropertyChanged("CarNumber");
            }
        }

        public double Tara
        {
            get { return _tara; }
            set
            {
                if (value.Equals(_tara))
                    return;
                _tara = value;
                RaisePropertyChanged("Tara");
            }
        }

        public Employee Driver
        {
            get { return _driver; }
            set
            {
                if (Equals(value, _driver))
                    return;
                _driver = value;
                RaisePropertyChanged("Driver");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Transport(Id);

            Container.Name = Name;
            Container.Number = CarNumber;
            Container.Tara = Tara;
            Container.DriverId = Driver != null ? Driver.Id : (Guid?)null;
        }

    }
}
