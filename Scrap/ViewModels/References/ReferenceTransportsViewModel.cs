using System.Collections.ObjectModel;
using Scrap.Core.Classes.References;
using Scrap.Models.References;
using Scrap.ViewModels.Base;
using Scrap.Views.References;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.References
{
    /// <summary>
    /// Модель представления справочника "Траснспорт"
    /// </summary>
    public class ReferenceTransportsViewModel : BaseEditorViewModel<TransportWrapper>
    {
        public ReferenceTransportsViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceTransportsView))
        {
            Title = "Справочник: автотранспорт";

            foreach (var transport in MainStorage.Instance.Transports)
                Items.Add(new TransportWrapper(transport));
        }

        public ReadOnlyObservableCollection<Employee> Drivers
        {
            get { return MainStorage.Instance.Drivers; }
        }

        protected override void AddItem()
        {
            TransportWrapper driver = new TransportWrapper();
            Items.Add(driver);
            SelectedItem = driver;
        }
    }
}
