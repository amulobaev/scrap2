using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public sealed class ReferenceBasesViewModel : BaseReferenceViewModel<OrganizationWrapper>
    {
        public ReferenceBasesViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceBasesView))
        {
            Title = "Справочник: базы";

            foreach (var organization in MainStorage.Instance.Bases)
                Items.Add(new OrganizationWrapper(organization));
        }

        protected override void AddItem()
        {
            OrganizationWrapper newItem = new OrganizationWrapper(OrganizationType.Base, "Новая база");
            Items.Add(newItem);
            SelectedItem = newItem;
        }

    }
}
