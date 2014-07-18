using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.Documents;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentProcessingViewModel : BaseDocumentViewModel<Processing, ProcessingItemWrapper>
    {
        private Employee _responsiblePerson;
        private string _comment;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public DocumentProcessingViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(DocumentProcessingView), id)
        {
            Title = "Переработка";

            if (Id != Guid.Empty)
            {
                // Загрузка документа
                LoadDocument();
            }
            else
            {
                // Новый документ
                Id = Guid.NewGuid();
                Number = MainStorage.Instance.DocumentsRepository.GetNextDocumentNumber();
                Date = DateTime.Now;
            }
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
            set
            {
                if (Equals(value, _responsiblePerson))
                    return;
                _responsiblePerson = value;
                RaisePropertyChanged("ResponsiblePerson");
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment)
                    return;
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        #region Методы

        protected override void SaveDocument()
        {
            if (!this.IsValid())
            {
                MessageBox.Show(this.Error);
                return;
            }

            bool isNew = false;
            if (Document == null)
            {
                isNew = true;
                Document = new Processing(Id);
            }

            Document.UserId = null;
            Document.Type = DocumentType.Processing;
            Document.Number = Number;
            Document.Date = Date.Value;
            Document.ResponsiblePersonId = ResponsiblePerson.Id;
            Document.Comment = Comment;

            if (Document.Items.Any())
                Document.Items.Clear();

            foreach (ProcessingItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Document.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.ProcessingRepository.Create(Document);
            else
                MainStorage.Instance.ProcessingRepository.Update(Document);
        }

        protected override void AddItem()
        {
            var newItem = new ProcessingItemWrapper();
            Items.Add(newItem);
        }

        private void LoadDocument()
        {
            Document = MainStorage.Instance.ProcessingRepository.GetById(Id);
            Number = Document.Number;
            Date = Document.Date;
            ResponsiblePerson = ResponsiblePersons.FirstOrDefault(x => x.Id == Document.ResponsiblePersonId);
            Comment = Document.Comment;

            foreach (ProcessingItem item in Document.Items)
                Items.Add(new ProcessingItemWrapper(item));
        }

        #endregion

    }
}
