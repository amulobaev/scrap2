using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Zlatmet2.Classes
{
    /// <summary>
    /// Базовый класс для моделей с валидацией
    /// </summary>
    public abstract class BaseValidationModel : BaseModel, IDataErrorInfo
    {
        private readonly Dictionary<string, Func<BaseValidationModel, object>> _propertyGetters;

        private readonly Dictionary<string, ValidationAttribute[]> _validators;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected BaseValidationModel()
        {
            _validators =
               this.GetType()
                   .GetProperties()
                   .Where(p => this.GetValidations(p).Length != 0)
                   .ToDictionary(p => p.Name, p => GetValidations(p));

            _propertyGetters =
                this.GetType()
                    .GetProperties()
                    .Where(p => this.GetValidations(p).Length != 0)
                    .ToDictionary(p => p.Name, p => GetValueGetter(p));
        }

        /// <summary>
        /// Количество валидных свойств
        /// </summary>
        public int ValidPropertiesCount
        {
            get
            {
                return _validators.Count(x => x.Value.All(attribute => attribute.IsValid(_propertyGetters[x.Key](this))));
            }
        }

        /// <summary>
        /// Всего свойств с валидацией
        /// </summary>
        public int TotalPropertiesWithValidationCount
        {
            get { return _validators.Count(); }
        }

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                if (_propertyGetters.ContainsKey(columnName))
                {
                    var propertyValue = _propertyGetters[columnName](this);
                    var errorMessages = _validators[columnName]
                        .Where(v => !v.IsValid(propertyValue))
                        .Select(v => v.ErrorMessage).ToArray();

                    return string.Join(Environment.NewLine, errorMessages);
                }

                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                var errors = from validator in _validators
                             from attribute in validator.Value
                             where !attribute.IsValid(_propertyGetters[validator.Key](this))
                             select attribute.ErrorMessage;

                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        #endregion

        #region Методы

        private ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        private Func<BaseValidationModel, object> GetValueGetter(PropertyInfo property)
        {
            return new Func<BaseValidationModel, object>(viewmodel => property.GetValue(viewmodel, null));
        }

        public virtual bool IsValid()
        {
            return string.IsNullOrEmpty(Error) && ValidPropertiesCount == TotalPropertiesWithValidationCount;
        }

        #endregion

    }
}