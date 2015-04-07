using System.Windows.Controls;
using System.Windows.Input;

namespace Scrap.Views.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportNomenclatureView.xaml
    /// </summary>
    public partial class ReportNomenclatureView : UserControl
    {
        public ReportNomenclatureView()
        {
            InitializeComponent();

            FocusManager.SetFocusedElement(GridBase, DatePickerFrom);
        }
    }
}
