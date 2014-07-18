using System;
using System.Collections.Generic;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentProcessing")]
    public class ProcessingDto : BaseDto
    {
        private List<ProcessingItemDto> _items;

        public Guid? UserId { get; set; }

        public int Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public Guid? ResponsiblePersonId { get; set; }

        public string Comment { get; set; }

        public List<ProcessingItemDto> Items
        {
            get { return _items ?? (_items = new List<ProcessingItemDto>()); }
        }
    }
}
