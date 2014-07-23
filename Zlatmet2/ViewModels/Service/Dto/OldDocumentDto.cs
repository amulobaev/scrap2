using System;
using Zlatmet2.Domain.Dto;

namespace Zlatmet2.ViewModels.Service.Dto
{
    [Table("documents")]
    class OldDocumentDto : BaseDto
    {
        public Guid doc_id { get; set; }
        public Guid? user_id { get; set; }
        public int doc_type { get; set; }
        public int number { get; set; }
        public DateTime doc_date { get; set; }
        public DateTime datapog { get; set; }
        public DateTime dataraz { get; set; }
        public Guid? supplier_id { get; set; }
        public Guid? customer_id { get; set; }
        public Guid? responsible_id { get; set; }
        public Guid? transport_id { get; set; }
        public Guid? driver_id { get; set; }
        public string psa { get; set; }
        public string ttn { get; set; }
        public string comment { get; set; }
        public int transport_type { get; set; }
        public string vagon { get; set; }
    }
}
