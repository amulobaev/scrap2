using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Zlatmet2.Annotations;
using Zlatmet2.Models;

namespace Zlatmet2.Classes
{
    /// <summary>
    /// Базовый класс для моделей
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        private bool _isChanged;

        private readonly List<string> _ignoreProperties;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected BaseModel()
        {
            // Готовим список свойств, при изменении которых нам не нужно выставлять флаг IsChanged = true;
            _ignoreProperties =
                this.GetType()
                    .GetProperties()
                    .Where(x => x.GetCustomAttributes(typeof(IgnoreChangesAttribute), true).Any())
                    .Select(x => x.Name)
                    .ToList();
        }


        [IgnoreChanges]
        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (value.Equals(_isChanged))
                    return;
                _isChanged = value;
                RaisePropertyChanged("IsChanged");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));

            if (_ignoreProperties.All(x => x != propertyName))
                IsChanged = true;
        }

        #endregion

    }
}
