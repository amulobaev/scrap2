using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Scrap.Core.Classes.Documents;
using Scrap.Core.Classes.References;
using Scrap.Core.Enums;
using Scrap.Models.Documents;
using Scrap.Views.Documents;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Documents
{
    public class DocumentProcessingViewModel : BaseDocumentViewModel<Processing, ProcessingItemWrapper>
    {
        #region Поля

        private DateTime? _date;

        private Employee _responsiblePerson;

        private string _comment;

        private Organization _base;

        #endregion Поля

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public DocumentProcessingViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(DocumentProcessingView), id)
        {
            if (Id != Guid.Empty)
            {
                // Загрузка документа
                LoadDocument(id);
            }
            else
            {
                // Новый документ
                Id = Guid.NewGuid();

                Guid idToLoad;
                if (optional != null && Guid.TryParse(optional.ToString(), out idToLoad))
                {
                    LoadDocument(idToLoad);
                    Container = null;
                }

                Number = MainStorage.Instance.JournalRepository.GetNextDocumentNumber();
                Date = DateTime.Now;
            }

            UpdateTitle();
        }

        #region Свойства

        /// <summary>
        /// Дата документа
        /// </summary>
        [Required]
        public DateTime? Date
        {
            get { return _date; }
            set { Set(() => Date, ref _date, value); }
        }

        public ReadOnlyObservableCollection<Organization> Bases
        {
            get { return MainStorage.Instance.Bases; }
        }

        [Required(ErrorMessage = @"Не выбрана база")]
        public Organization Base
        {
            get { return _base; }
            set { Set(() => Base, ref _base, value); }
        }

        /// <summary>
        /// Ответственные лица
        /// </summary>
        public ReadOnlyObservableCollection<Employee> ResponsiblePersons
        {
            get { return MainStorage.Instance.ResponsiblePersons; }
        }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        [Required(ErrorMessage = @"Не выбрано ответственное лицо")]
        public Employee ResponsiblePerson
        {
            get { return _responsiblePerson; }
            set { Set(() => ResponsiblePerson, ref _responsiblePerson, value); }
        }

        public string Comment
        {
            get { return _comment; }
            set { Set(() => Comment, ref _comment, value); }
        }

        protected override string DocumentTitle
        {
            get { return "Переработка"; }
        }

        #endregion Свойства

        #region Методы

        protected override void SaveDocument()
        {
            if (!this.IsValid())
            {
                MessageBox.Show(this.Error);
                return;
            }

            bool isNew = false;
            if (Container == null)
            {
                isNew = true;
                Container = new Processing(Id);
            }

            Container.UserId = null;
            Container.Type = DocumentType.Processing;
            Container.Number = Number;
            Container.Date = Date.Value;
            Container.BaseId = Base.Id;
            Container.ResponsiblePersonId = ResponsiblePerson.Id;
            Container.Comment = Comment;

            if (Container.Items.Any())
                Container.Items.Clear();

            foreach (ProcessingItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Container.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.ProcessingRepository.Create(Container);
            else
                MainStorage.Instance.ProcessingRepository.Update(Container);
        }

        protected override void AddItem()
        {
            var newItem = new ProcessingItemWrapper();
            Items.Add(newItem);
        }

        protected override void UpdateTitle()
        {
            Title = string.Format("{0} №{1} от {2}", DocumentTitle, Number,
                Date.HasValue ? Date.Value.ToShortDateString() : string.Empty);
        }

        private void LoadDocument(Guid id)
        {
            Container = MainStorage.Instance.ProcessingRepository.GetById(id);
            Number = Container.Number;
            Date = Container.Date;
            Base = Bases.FirstOrDefault(x => x.Id == Container.BaseId);
            ResponsiblePerson = ResponsiblePersons.FirstOrDefault(x => x.Id == Container.ResponsiblePersonId);
            Comment = Container.Comment;

            foreach (ProcessingItem item in Container.Items)
                Items.Add(new ProcessingItemWrapper(item));
        }

        #endregion

    }
}
