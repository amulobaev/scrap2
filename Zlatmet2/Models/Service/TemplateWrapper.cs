using System;
using System.ComponentModel.DataAnnotations;
using Zlatmet2.Core.Classes.Service;

namespace Zlatmet2.Models.Service
{
    /// <summary>
    /// Обёртка для шаблона
    /// </summary>
    public sealed class TemplateWrapper : BaseReferenceWrapper<Template>
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
            if (dataForContainer == null)
            {
                // Новый шаблон
                Id = Guid.NewGuid();
                Name = "Новый шаблон";
            }
            else
            {
                Id = Container.Id;
                _name = Container.Name;
                _data = Container.Data;
            }
        }

        [Required]
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
        
    }
}