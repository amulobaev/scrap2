﻿using System.Windows.Controls;
using System.Windows.Input;

namespace Scrap.Views.Documents
{
    /// <summary>
    /// Логика взаимодействия для DocumentRemainsView.xaml
    /// </summary>
    public partial class DocumentRemainsView : UserControl
    {
        public DocumentRemainsView()
        {
            InitializeComponent();

            FocusManager.SetFocusedElement(GroupBoxBase, UpDownNumber);
        }
    }
}
