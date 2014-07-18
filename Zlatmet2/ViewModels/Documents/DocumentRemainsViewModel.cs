﻿using System;
using System.Linq;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.Documents;
using Zlatmet2.Views.Documents;

namespace Zlatmet2.ViewModels.Documents
{
    public class DocumentRemainsViewModel : BaseDocumentViewModel<Remains, RemainsItemWrapper>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        public DocumentRemainsViewModel(LayoutDocument layout, Guid id)
            : base(layout, typeof(DocumentRemainsView), id)
        {
            Title = "Корректировка остатков";

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

        private void LoadDocument()
        {
            Document = MainStorage.Instance.RemainsRepository.GetById(Id);
            Number = Document.Number;
            Date = Document.Date;

            foreach (RemainsItem item in Document.Items)
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
            if (Document == null)
            {
                isNew = true;
                Document = new Remains(Id);
            }

            Document.UserId = null;
            Document.Type = DocumentType.Remains;
            Document.Number = Number;
            Document.Date = Date.Value;

            if (Document.Items.Any())
                Document.Items.Clear();

            foreach (RemainsItemWrapper itemWrapper in Items)
            {
                itemWrapper.UpdateContainer();
                Document.Items.Add(itemWrapper.Container);
            }

            if (isNew)
                MainStorage.Instance.RemainsRepository.Create(Document);
            else
                MainStorage.Instance.RemainsRepository.Update(Document);
        }

        protected override void AddItem()
        {
            var newItem = new RemainsItemWrapper();
            Items.Add(newItem);
        }

    }
}
