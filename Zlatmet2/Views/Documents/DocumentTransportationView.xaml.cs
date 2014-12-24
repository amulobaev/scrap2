﻿using System.Windows.Controls;
using System.Windows.Input;

namespace Zlatmet2.Views.Documents
{
    /// <summary>
    /// Логика взаимодействия для DocumentTransportationView.xaml
    /// </summary>
    public partial class DocumentTransportationView : UserControl
    {
        public DocumentTransportationView()
        {
            InitializeComponent();

            FocusManager.SetFocusedElement(GroupBoxBase, UpDownNumber);
        }
    }
}
