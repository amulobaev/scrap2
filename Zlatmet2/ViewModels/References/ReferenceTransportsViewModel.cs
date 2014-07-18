using System.Collections.ObjectModel;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceTransportsViewModel : BaseReferenceViewModel<TransportWrapper>
    {
        public ReferenceTransportsViewModel(LayoutDocument layout)
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
