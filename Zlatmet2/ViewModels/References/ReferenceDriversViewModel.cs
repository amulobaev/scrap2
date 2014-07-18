using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceDriversViewModel : BaseReferenceViewModel<EmployeeWrapper>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        public ReferenceDriversViewModel(LayoutDocument layout)
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