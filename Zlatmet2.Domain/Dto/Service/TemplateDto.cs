using System;

namespace Zlatmet2.Domain.Dto.Service
{
    [Table("Templates")]
    public class TemplateDto : BaseDto
    {
        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}
