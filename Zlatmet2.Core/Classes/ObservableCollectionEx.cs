using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Zlatmet2.Core.Classes
{
    /// <summary>
    /// Наследник ObservableCollection с ускоренными "групповыми" операциями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> list)
        {
            foreach (T item in list)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveAll(Func<T, bool> condition)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (condition(Items[i]))
                    Items.RemoveAt(i);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}
