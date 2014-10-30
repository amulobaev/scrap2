using System.Windows.Controls;
using Stimulsoft.Report;

namespace Zlatmet2.Views.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportRemainsView.xaml
    /// </summary>
    public partial class ReportRemainsView : UserControl
    {
        public ReportRemainsView()
        {
            InitializeComponent();

            StiViewer.Report = new StiReport();
        }
    }
}
