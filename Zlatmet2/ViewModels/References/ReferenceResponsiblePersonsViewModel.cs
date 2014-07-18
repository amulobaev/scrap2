using System;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public class ReferenceResponsiblePersonsViewModel : BaseReferenceViewModel<EmployeeWrapper>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        public ReferenceResponsiblePersonsViewModel(LayoutDocument layout)
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