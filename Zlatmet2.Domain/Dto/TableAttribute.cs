using System;

namespace Zlatmet2.Domain.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class TableAttribute : Attribute
    {
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        public string Name { get; set; }
    }
}
