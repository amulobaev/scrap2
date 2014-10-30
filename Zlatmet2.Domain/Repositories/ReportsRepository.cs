using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Zlatmet2.Core;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Core.Classes.Reports;

namespace Zlatmet2.Domain.Repositories
{
    /// <summary>
    /// Репозитарий для отчётов
    /// </summary>
    public class ReportsRepository : BaseRepository
    {
        static ReportsRepository()
        {
        }

        public ReportsRepository(IModelContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Формирование данных для отчёта "Остатки на базе"
        /// </summary>
        /// <param name="date"></param>
        /// <param name="bases"></param>
        /// <param name="nomenclatures"></param>
        /// <returns></returns>
        public List<ReportRemainsBase> ReportRemains(DateTime date, IEnumerable<Organization> bases,
            IEnumerable<Guid> nomenclatures)
        {
            List<ReportRemainsBase> reportData = new List<ReportRemainsBase>();

            string nomenclature = string.Join(",", nomenclatures.Select(x => "'" + x.ToString() + "'").ToList());

            using (IDbConnection connection = ConnectionFactory.Create())
            {
                foreach (Organization organization in bases)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Date", date, DbType.Date);
                    parameters.Add("@Base", organization.Id, DbType.Guid);
                    parameters.Add("@Nomenclatures", nomenclature, DbType.String);

                    List<ReportRemainsData> data =
                        connection.Query<ReportRemainsData>("ReportRemains", parameters,
                            commandType: CommandType.StoredProcedure).ToList();
                    reportData.Add(new ReportRemainsBase(organization.Name, data));
                }
            }

            return reportData;
        }


        public List<ReportNomenclatureData> ReportNomenclature(DateTime dateFrom, DateTime dateTo)
        {
            using (IDbConnection connection = ConnectionFactory.Create())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DateFrom", dateFrom, DbType.Date);
                parameters.Add("@DateTo", dateTo, DbType.Date);

                List<ReportNomenclatureData> data =
                    connection.Query<ReportNomenclatureData>("ReportNomenclature", parameters, commandType: CommandType.StoredProcedure)
                        .ToList();
                for (int i = 0; i < data.Count; i++)
                    data[i].Number = i + 1;

                return data;
            }
        }

    }
}
