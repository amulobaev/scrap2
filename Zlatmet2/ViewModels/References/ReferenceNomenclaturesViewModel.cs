using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceNomenclaturesViewModel : BaseReferenceViewModel<NomenclatureWrapper>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        public ReferenceNomenclaturesViewModel(LayoutDocument layout)
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
