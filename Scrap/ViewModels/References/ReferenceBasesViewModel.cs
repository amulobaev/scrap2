using Scrap.Core.Enums;
using Scrap.Models.References;
using Scrap.ViewModels.Base;
using Scrap.Views.References;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.References
{
    public sealed class ReferenceBasesViewModel : BaseEditorViewModel<OrganizationWrapper>
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
