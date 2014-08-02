using System;
using System.Windows;

namespace Zlatmet2.Tools
{
    /// <summary>
    /// Вспомогательный метод для WPF Dispatcher.
    /// Работает с объектами из другого потока.
    /// </summary>
    public static class ThreadContext
    {
        public static void InvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
                action();
            else
                Application.Current.Dispatcher.Invoke(action);
        }

        public static void BeginInvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
                action();
            else
                Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
