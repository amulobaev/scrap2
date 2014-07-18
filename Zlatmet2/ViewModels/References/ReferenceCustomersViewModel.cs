using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceCustomersViewModel : ReferenceOrganizationsViewModel
    {
        public ReferenceCustomersViewModel(LayoutDocument layout)
            : base(layout)
        {
            Title = "Справочник: заказчики";

            foreach (var customer in MainStorage.Instance.Customers)
                Items.Add(new OrganizationWrapper(customer));
        }

        protected override void AddItem()
        {
            OrganizationWrapper customer = new OrganizationWrapper(OrganizationType.Customer, "Новый заказчик");
            customer.Divisions.Add(new DivisionWrapper(customer.Id, 1, "Основное"));
            Items.Add(customer);
            SelectedItem = customer;
        }

    }
}