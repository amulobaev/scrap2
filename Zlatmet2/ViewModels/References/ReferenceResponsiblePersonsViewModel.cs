using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceResponsiblePersonsViewModel : BaseEditorViewModel<EmployeeWrapper>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public ReferenceResponsiblePersonsViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceEmployeesView))
        {
            Title = "Справочник: ответственные лица";

            foreach (var responsiblePerson in MainStorage.Instance.ResponsiblePersons)
                Items.Add(new EmployeeWrapper(responsiblePerson));
        }

        protected override void AddItem()
        {
            EmployeeWrapper responsiblePerson = new EmployeeWrapper(EmployeeType.Responsible, "Новое ответственное лицо");
            Items.Add(responsiblePerson);
            SelectedItem = responsiblePerson;
        }
    }
}