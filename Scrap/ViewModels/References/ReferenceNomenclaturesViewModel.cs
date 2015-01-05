using Scrap.Models.References;
using Scrap.ViewModels.Base;
using Scrap.Views.References;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.References
{
    public class ReferenceNomenclaturesViewModel : BaseEditorViewModel<NomenclatureWrapper>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public ReferenceNomenclaturesViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceNomenclaturesView))
        {
            Title = "Справочник: номенклатура";

            foreach (var nomenclature in MainStorage.Instance.Nomenclatures)
                Items.Add(new NomenclatureWrapper(nomenclature));
        }

        protected override void AddItem()
        {
            NomenclatureWrapper newItem = new NomenclatureWrapper();
            Items.Add(newItem);
            SelectedItem = newItem;
        }

    }
}
