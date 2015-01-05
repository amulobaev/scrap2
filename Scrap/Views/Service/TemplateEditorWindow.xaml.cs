using System.Windows;
using Scrap.Models.Service;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.WpfDesign;

namespace Scrap.Views.Service
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditorWindow.xaml
    /// </summary>
    public partial class TemplateEditorWindow : Window
    {
        private readonly TemplateWrapper _templateWrapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="templateWrapper"></param>
        public TemplateEditorWindow(TemplateWrapper templateWrapper)
        {
            InitializeComponent();

            //StiOptions.Designer.MainMenu.ShowFilePageSaveAs = false;
            StiOptions.Engine.GlobalEvents.SavingReportInDesigner += GlobalEvents_SavingReportInDesigner;

            _templateWrapper = templateWrapper;

            DesignerControl.Report = new StiReport();
            DesignerControl.Report.Load(templateWrapper.Data);
        }

        private void GlobalEvents_SavingReportInDesigner(object sender, StiSavingObjectEventArgs e)
        {
            StiWpfDesignerControl designerControl = sender as StiWpfDesignerControl;
            if (designerControl == null)
                return;

            _templateWrapper.Data = designerControl.Report.SaveToByteArray();
            e.Processed = true;
        }

    }
}
