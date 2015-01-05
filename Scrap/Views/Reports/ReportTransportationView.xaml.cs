using System.Windows.Controls;
using System.Windows.Input;

namespace Scrap.Views.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportTransportationView.xaml
    /// </summary>
    public partial class ReportTransportationView : UserControl
    {
        public ReportTransportationView()
        {
            InitializeComponent();

            FocusManager.SetFocusedElement(GridBase, CheckBoxIsAuto);
        }
    }
}
