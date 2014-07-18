using System;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportCustomerViewModel : UniqueLayoutDocumentViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public ReportCustomerViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(ReportCustomerView), id)
        {
            Title = "Отчёт по заказчику";
            Id = Guid.NewGuid();
        }
    }
}
