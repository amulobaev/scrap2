using System;
using System.Windows;
using Zlatmet2.ViewModels.References;

namespace Zlatmet2.Views.References
{
    /// <summary>
    /// Логика взаимодействия для ConvertContractorToDivisionWindow.xaml
    /// </summary>
    public partial class ConvertContractorToDivisionWindow : Window
    {
        public ConvertContractorToDivisionWindow(Guid contractorId)
        {
            InitializeComponent();

            this.DataContext = new ConvertContractorToDivisionViewModel(contractorId);
        }

        public ConvertContractorToDivisionViewModel ViewModel
        {
            get { return this.DataContext as ConvertContractorToDivisionViewModel; }
        }

    }
}
