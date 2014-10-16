using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GalaSoft.MvvmLight;
using Zlatmet2.Models;

namespace Zlatmet2.Classes
{
    /// <summary>
    /// Базовый класс для моделей
    /// </summary>
    public abstract class BaseModel : ObservableObject
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
            set { Set(() => IsChanged, ref _isChanged, value); }
        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);

            if (!InIgnoreList(propertyName))
                IsChanged = true;
        }

        protected override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.RaisePropertyChanged(propertyExpression);

            string propertyName = GetPropertyName(propertyExpression);
            if (!InIgnoreList(propertyName))
                IsChanged = true;
        }

        private bool InIgnoreList(string propertyName)
        {
            return _ignoreProperties.Any(x => string.Equals(x, propertyName));
        }

    }
}
