using System;
using System.ComponentModel;
using Zlatmet2.Annotations;

namespace Zlatmet2.Models.Reports
{
    public class CheckedWrapper : INotifyPropertyChanged
    {
        public CheckedWrapper()
        {
        }

        public CheckedWrapper(Guid id, bool isChecked, string text)
        {
            Id = id;
            IsChecked = isChecked;
            Text = text;
        }

        public Guid Id { get; set; }

        public bool IsChecked { get; set; }

        public string Text { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
