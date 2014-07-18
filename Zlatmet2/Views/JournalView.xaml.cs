using System.Windows.Controls;

namespace Zlatmet2.Views
{
    /// <summary>
    /// Логика взаимодействия для JournalView.xaml
    /// </summary>
    public partial class JournalView : UserControl
    {
        public JournalView()
        {
            InitializeComponent();
        }

        //private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    Update();
        //}

        //private void Update()
        //{
        //var dateFrom = CheckBoxInterval.IsChecked == true && DatePickerFrom.SelectedDate.HasValue
        //    ? DatePickerFrom.SelectedDate.Value
        //    : (DateTime?)null;
        //var dateTo = CheckBoxInterval.IsChecked == true && DatePickerTo.SelectedDate.HasValue
        //    ? DatePickerTo.SelectedDate.Value
        //    : (DateTime?)null;
        //var backgroundWorker = new BackgroundWorker();

        //backgroundWorker.DoWork += (sender, args) =>
        //{
        //    using (var context = new ZlatmetEntities())
        //    {
        //        var documents = (from x in context.usp_GetDocuments(dateFrom, dateTo)
        //                         select
        //                             new Document
        //                             {
        //                                 DocId = x.doc_id,
        //                                 Number = x.number,
        //                                 Date = x.doc_date,
        //                                 DocType = (DocumentType)x.doc_type,
        //                                 DocTypeS =
        //                                     (DocumentType)x.doc_type == DocumentType.Transportation
        //                                         ? string.Format("{0} {1}", x.doc_type_s, Helpers.GetTransportType(x.transport_type))
        //                                         : x.doc_type_s,
        //                                 Supplier = x.supplier,
        //                                 Customer = x.customer,
        //                                 Responsible = x.responsible,
        //                                 Psa = x.psa,
        //                                 Comment = x.comment,
        //                                 Netto = x.netto,
        //                                 Nomenclature = x.nom
        //                             }).ToList();
        //        DispatcherHelper.CheckBeginInvokeOnUI(() =>
        //        {
        //            DataGridDocuments.ItemsSource = new ObservableCollection<Document>(documents);
        //        });
        //    }
        //};

        //backgroundWorker.RunWorkerCompleted += (sender, args) =>
        //{
        //    BusyIndicator.IsBusy = false;
        //};

        //BusyIndicator.IsBusy = true;
        //backgroundWorker.RunWorkerAsync();
        //}

    }
}