using System.Windows.Controls;
using Stimulsoft.Report;

namespace Zlatmet2.Views.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportWarehouseView.xaml
    /// </summary>
    public partial class ReportWarehouseView : UserControl
    {
        public ReportWarehouseView()
        {
            InitializeComponent();

            StiViewer.Report = new StiReport();
        }
    }
}
