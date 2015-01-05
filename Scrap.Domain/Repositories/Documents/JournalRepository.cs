using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using Scrap.Core.Classes.Documents;
using Scrap.Domain.Entities;

namespace Scrap.Domain.Repositories.Documents
{
    public class JournalRepository
    {
        static JournalRepository()
        {
            Mapper.CreateMap<DocumentEntity, Document>();
        }

        public JournalRepository()
        {
        }

        public IEnumerable<Document> GetAll(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                List<string> parameters = new List<string>();
                List<object> sqlParameters = new List<object>();
                if (dateFrom.HasValue)
                {
                    parameters.Add("@DateFrom");
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = "@DateFrom",
                        SqlDbType = SqlDbType.Date,
                        Value = dateFrom.Value
                    });
                }
                if (dateTo.HasValue)
                {
                    parameters.Add("@DateTo");
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = "@DateTo",
                        SqlDbType = SqlDbType.Date,
                        Value = dateTo.Value
                    });
                }

                return context.Database.SqlQuery<Document>("GetDocuments " + string.Join(",", parameters),
                        sqlParameters.ToArray()).ToList();
            }
        }

        public int GetNextDocumentNumber()
        {
            int year = DateTime.Today.Year;

            using (ZlatmetContext context = new ZlatmetContext())
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Year",
                    SqlDbType = SqlDbType.Int,
                    Value = year
                };
                List<int?> items = context.Database.SqlQuery<int?>("GetMaxDocumentNumber @Year", parameter).ToList();
                return items.Any() && items[0].HasValue ? items[0].Value + 1 : 1;
            }
        }

    }
}