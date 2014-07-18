using System;
using System.ComponentModel;
using System.Reflection;

namespace Zlatmet2.Core.Tools
{
    public static class Helpers
    {
        public static string GetTransportType(int transportType)
        {
            switch (transportType)
            {
                case 0:
                    return "(авто)";
                case 1:
                    return "(ж/д)";
                default:
                    return string.Empty;
            }
        }

        public static string GetEnumDescription(Enum enumObj)
        {
            if (enumObj == null)
                throw new ArgumentNullException("enumObj");

            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var descriptionAttribute = attribArray[0] as DescriptionAttribute;
                return descriptionAttribute.Description;
            }
        }

    }
}
