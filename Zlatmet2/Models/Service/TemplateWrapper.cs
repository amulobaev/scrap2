using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zlatmet2.Core.Classes.Service;

namespace Zlatmet2.Models.Service
{
    /// <summary>
    /// Обёртка для шаблона
    /// </summary>
    public class TemplateWrapper : BaseValidationWrapper<Template>
    {
        private string _name;
        private byte[] _data;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataForContainer"></param>
        public TemplateWrapper(Template dataForContainer = null)
            : base(dataForContainer)
        {
            if (dataForContainer != null)
            {
                Id = Container.Id;
                _name = Container.Name;
                _data = Container.Data;
            }
            else
            {
                Id = Guid.NewGuid();
                Name = "Новый шаблон";
            }
        }

        [IgnoreChanges]
        public Guid Id { get; protected set; }

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

        public byte[] Data
        {
            get { return _data; }
            set
            {
                if (Equals(value, _data)) return;
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new Template(Id);
            Container.Name = Name;
            Container.Data = Data;
        }

        public void Save()
        {
            if (!IsChanged)
                return;

            bool isNew = Container == null;
            UpdateContainer();
            if (isNew)
                MainStorage.Instance.TemplatesRepository.Create(Container);
            else
                MainStorage.Instance.TemplatesRepository.Update(Container);
            
            IsChanged = false;
        }
    }
}