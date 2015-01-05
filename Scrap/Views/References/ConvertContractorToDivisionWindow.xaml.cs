using System;
using System.Windows;
using Scrap.ViewModels.References;

namespace Scrap.Views.References
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
