using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceSuppliersViewModel : ReferenceOrganizationsViewModel
    {
        public ReferenceSuppliersViewModel(LayoutDocument layout)
            : base(layout)
        {
            Title = "Справочник: поставщики";

            foreach (var supplier in MainStorage.Instance.Suppliers)
                Items.Add(new OrganizationWrapper(supplier));
        }

        protected override void AddItem()
        {
            OrganizationWrapper supplier = new OrganizationWrapper(OrganizationType.Supplier, "Новый поставщик");
            supplier.Divisions.Add(new DivisionWrapper(supplier.Id, 1, "Основное"));
            Items.Add(supplier);
            SelectedItem = supplier;
        }

    }
}