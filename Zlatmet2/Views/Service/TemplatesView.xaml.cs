using System.Windows.Controls;
using Stimulsoft.Report;
using Telerik.Windows.Controls;
using Zlatmet2.Models.Service;

namespace Zlatmet2.Views.Service
{
    /// <summary>
    /// Логика взаимодействия для TemplatesView.xaml
    /// </summary>
    public partial class TemplatesView : UserControl
    {
        public TemplatesView()
        {
            InitializeComponent();
        }

        private void GridViewTemplates_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            TemplateWrapper selectedTemplate = GridViewTemplates.SelectedItem as TemplateWrapper;
            if (selectedTemplate != null && selectedTemplate.Data != null)
            {
                StiReport report = new StiReport();
                report.Load(selectedTemplate.Data);
                report.Render();
                ReportViewer.Report = report;
            }
            else
            {
                ReportViewer.Report = null;
            }
        }
    }
}
