using System.Windows.Controls;
using System.Windows.Input;
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

            FocusManager.SetFocusedElement(GridBase, DatePicker);

            StiViewer.Report = new StiReport();
        }
    }
}
