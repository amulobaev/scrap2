using Scrap.Core.Enums;
using Scrap.Models.References;
using Scrap.ViewModels.Base;
using Scrap.Views.References;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.References
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