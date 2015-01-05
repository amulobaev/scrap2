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
    public class DocumentRemainsViewModel : BaseDocumentViewModel<Remains, RemainsItemWrapper>
    {
        private Organization _base;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public DocumentRemainsViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(DocumentRemainsView), id)
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

        protected override string DocumentTitle
        {
            get { return "Корректировка остатков"; }
        }

        public ReadOnlyObservableCollection<Organization> Bases
        {
            get { return MainStorage.Instance.Bases; }
        }

        [Required(ErrorMessage = @"Не выбрана база")]
        public Organization Base
        {
            get { return _base; }
            set
            {
                if (Equals(value, _base))
                    return;
                _base = value;
                RaisePropertyChanged("Base");
            }
        }
        private void LoadDocument(Guid id)
        {
            Container = MainStorage.Instance.RemainsRepository.GetById(id);
            Number = Container.Number;
            Date = Container.Date;
            Base = Bases.FirstOrDefault(x => x.Id == Container.BaseId);

            foreach (RemainsItem item in Container.Items)
                Items.Add(new RemainsItemWrapper(item));
        }

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
                Container = new Remains(Id);
            }

            Container.UserId = null;
            Container.Type = DocumentType.Remains;
            Container.Number = Number;
            Container.Date = Date.Value;
            Container.BaseId = Base.Id;

            if (Container.Items.Any())
                Container.Items.Clear();

            foreach (RemainsItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Container.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.RemainsRepository.Create(Container);
            else
                MainStorage.Instance.RemainsRepository.Update(Container);
        }

        protected override void AddItem()
        {
            var newItem = new RemainsItemWrapper();
            Items.Add(newItem);
        }

    }
}
