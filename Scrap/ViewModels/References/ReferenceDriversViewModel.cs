using Scrap.Core.Enums;
using Scrap.Models.References;
using Scrap.ViewModels.Base;
using Scrap.Views.References;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.References
{
    /// <summary>
    /// Модель представления справочника "Водители"
    /// </summary>
    public class ReferenceDriversViewModel : BaseEditorViewModel<EmployeeWrapper>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public ReferenceDriversViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceEmployeesView))
        {
            Title = "Справочник: водители";

            foreach (var driver in MainStorage.Instance.Drivers)
                Items.Add(new EmployeeWrapper(driver));
        }

        protected override void AddItem()
        {
            EmployeeWrapper driver = new EmployeeWrapper(EmployeeType.Driver, "Новый водитель");
            Items.Add(driver);
            SelectedItem = driver;
        }
    }
}