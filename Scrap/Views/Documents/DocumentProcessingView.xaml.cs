﻿using System.Windows.Controls;
using System.Windows.Input;

namespace Scrap.Views.Documents
{
    /// <summary>
    /// Логика взаимодействия для DocumentProcessingView.xaml
    /// </summary>
    public partial class DocumentProcessingView : UserControl
    {
        public DocumentProcessingView()
        {
            InitializeComponent();

            FocusManager.SetFocusedElement(GroupBoxBase, UpDownNumber);
        }
    }
}