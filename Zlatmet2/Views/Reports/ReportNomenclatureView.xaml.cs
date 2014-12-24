using System.Windows.Controls;
using System.Windows.Input;
using Stimulsoft.Report;

namespace Zlatmet2.Views.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportNomenclatureView.xaml
    /// </summary>
    public partial class ReportNomenclatureView : UserControl
    {
        public ReportNomenclatureView()
        {
            InitializeComponent();

            StiViewer.Report = new StiReport();

            FocusManager.SetFocusedElement(GridBase, DatePickerFrom);
        }
    }
}
