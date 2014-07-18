using System;
using System.Collections.Generic;

namespace Zlatmet2.Domain.Dto.Documents
{
    [Table("DocumentRemains")]
    public class RemainsDto : BaseDto
    {
        private List<RemainsItemDto> _items;

        public Guid? UserId { get; set; }

        public int Type { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public List<RemainsItemDto> Items
        {
            get { return _items ?? (_items = new List<RemainsItemDto>()); }
        }
    }
}