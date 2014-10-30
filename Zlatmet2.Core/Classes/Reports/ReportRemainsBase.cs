using System.Collections.Generic;
using System.Linq;

namespace Zlatmet2.Core.Classes.Reports
{
    public class ReportRemainsBase
    {
        private readonly List<ReportRemainsData> _remains = new List<ReportRemainsData>();

        public ReportRemainsBase(string name, IList<ReportRemainsData> data)
        {
            Name = name;
            if (data != null && data.Any())
                _remains.AddRange(data);
        }

        public string Name { get; set; }

        public IEnumerable<ReportRemainsData> Remains
        {
            get { return _remains; }
        }
    }
}
