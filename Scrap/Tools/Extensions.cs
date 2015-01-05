using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scrap.Core;

namespace Scrap.Tools
{
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
                source.Add(item);
        }

        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (condition(collection[i]))
                    collection.RemoveAt(i);
            }
        }

        public static Guid? GetId(this PersistentObject persistentObject)
        {
            return persistentObject != null ? persistentObject.Id : (Guid?)null;
        }
    }
}
