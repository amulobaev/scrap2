using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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
            return attribArray.Length == 0 ? enumObj.ToString() : ((DescriptionAttribute)attribArray[0]).Description;
        }

        public static string Sha1Pass(string pass)
        {
            var sha1Managed = new SHA1Managed();
            Byte[] result = sha1Managed.ComputeHash(new UTF8Encoding().GetBytes(pass));
            var hashedString = new StringBuilder();
            foreach (Byte outputByte in result)
                hashedString.Append(outputByte.ToString("x2").ToUpper());
            return hashedString.ToString();
        }

    }
}
