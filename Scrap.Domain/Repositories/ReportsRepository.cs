﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Reports;
using Scrap.Core.Enums;

namespace Scrap.Domain.Repositories
{
    /// <summary>
    /// Репозитарий для отчётов
    /// </summary>
    public class ReportsRepository
    {
        static ReportsRepository()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ReportsRepository()
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

            using (ZlatmetContext context = new ZlatmetContext())
            {
                foreach (Organization organization in bases)
                {
                    object[] parameters = 
                    {
                        new SqlParameter
                        {
                            ParameterName = "@Date",
                            SqlDbType = SqlDbType.Date,
                            Value = date
                        },
                        new SqlParameter
                        {
                            ParameterName = "@Base",
                            Value = organization.Id
                        },
                        new SqlParameter
                        {
                            ParameterName = "@Nomenclatures",
                            Value = nomenclature
                        }
                    };

                    string query = string.Format("ReportRemains {0}",
                        string.Join(",", parameters.OfType<SqlParameter>().Select(x => x.ParameterName)));
                    List<ReportRemainsData> data =
                        context.Database.SqlQuery<ReportRemainsData>(query, parameters).ToList();
                    reportData.Add(new ReportRemainsBase(organization.Name, data));
                }
            }

            return reportData;
        }

        /// <summary>
        /// Формирование данных для отчета "Обороты за период"
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="isBases"></param>
        /// <param name="bases">Идентификаторы выбранных баз</param>
        /// <param name="isTransit"></param>
        /// <returns></returns>
        public List<ReportNomenclatureData> ReportNomenclature(DateTime dateFrom, DateTime dateTo, bool isBases,
            IEnumerable<Guid> bases, bool isTransit)
        {
            string baseIds = string.Join(",", bases.Select(x => "'" + x.ToString() + "'").ToList());

            using (ZlatmetContext context = new ZlatmetContext())
            {
                object[] parameters =
                {
                    new SqlParameter
                    {
                        ParameterName = "@DateFrom",
                        SqlDbType = SqlDbType.Date,
                        Value = dateFrom
                    },
                    new SqlParameter
                    {
                        ParameterName = "@DateTo",
                        SqlDbType = SqlDbType.Date,
                        Value = dateTo
                    },
                    new SqlParameter
                    {
                        ParameterName = "@IsBases",
                        Value = isBases
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Bases",
                        Value = baseIds
                    },
                    new SqlParameter
                    {
                        ParameterName = "@IsTransit",
                        Value = isTransit
                    }
                };

                string query = string.Format("ReportNomenclature {0}",
                    string.Join(",", parameters.OfType<SqlParameter>().Select(x => x.ParameterName)));

                List<ReportNomenclatureData> data =
                    context.Database.SqlQuery<ReportNomenclatureData>(query, parameters).ToList();
                for (int i = 0; i < data.Count; i++)
                    data[i].Number = i + 1;

                return data;
            }
        }

        /// <summary>
        /// Формирование данных для отчете "Перевозки"
        /// </summary>
        /// <param name="isAuto"></param>
        /// <param name="isTrain"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reportType"></param>
        /// <param name="supplierDivisions"></param>
        /// <param name="customerDivisions"></param>
        /// <param name="nomenclatures"></param>
        /// <param name="transports"></param>
        /// <returns></returns>
        public List<ReportTransportationData> ReportTransportation(bool isAuto, bool isTrain, DateTime? dateFrom,
            DateTime? dateTo, int reportType, Guid[] supplierDivisions, Guid[] customerDivisions, Guid[] nomenclatures,
            Guid[] transports)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                // Тип транспорта
                string transportType = isAuto && isTrain
                    ? (int) DocumentType.TransportationAuto + "," + (int) DocumentType.TransportationTrain
                    : isAuto
                        ? ((int) DocumentType.TransportationAuto).ToString()
                        : ((int) DocumentType.TransportationTrain).ToString();
                string supplierDivisionsString = supplierDivisions != null && supplierDivisions.Any()
                    ? string.Join(",", supplierDivisions.Select(x => "'" + x.ToString() + "'").ToList())
                    : null;
                string customerDivisionsString = customerDivisions != null && customerDivisions.Any()
                    ? string.Join(",", customerDivisions.Select(x => "'" + x.ToString() + "'").ToList())
                    : null;

                List<object> parameters = new List<object>()
                {
                    // TransportType
                    new SqlParameter("IsAuto", isAuto ? 1 : 0),
                    // TransportType
                    new SqlParameter("IsTrain", isTrain ? 1 : 0),
                    // DateFrom
                    new SqlParameter
                    {
                        ParameterName = "DateFrom",
                        SqlDbType = SqlDbType.Date,
                        Value = dateFrom.HasValue ? (object) dateFrom.Value : DBNull.Value
                    },
                    new SqlParameter
                    {
                        ParameterName = "DateTo",
                        SqlDbType = SqlDbType.Date,
                        Value = dateTo.HasValue ? (object) dateTo.Value : DBNull.Value
                    },
                    new SqlParameter("ReportType", reportType),
                    new SqlParameter("SupplierDivisions",
                        !string.IsNullOrEmpty(supplierDivisionsString) ? (object) supplierDivisionsString : DBNull.Value),
                    new SqlParameter("CustomerDivisions",
                        !string.IsNullOrEmpty(customerDivisionsString) ? (object) customerDivisionsString : DBNull.Value),
                    // Номенклатура
                    new SqlParameter("Nomenclatures",
                        string.Join(",", nomenclatures.Select(x => "'" + x.ToString() + "'").ToList())),
                    new SqlParameter("Transports", string.Join(",", transports.Select(x => "'" + x.ToString() + "'").ToList()))
                    
                };

                string query = string.Format("exec ReportTransportation {0}",
                    string.Join(",", parameters.OfType<SqlParameter>().Select(x => "@" + x.ParameterName)));
                List<ReportTransportationData> data =
                    context.Database.SqlQuery<ReportTransportationData>(query, parameters.ToArray()).ToList();
                for (int i = 0; i < data.Count; i++)
                    data[i].Number = i + 1;

                return data;
            }
        }

        /// <summary>
        /// ФОрмирование данных для отчёта по транспорту
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="transports"></param>
        /// <returns></returns>
        public List<ReportAutoTransportData> ReportAutoTransport(DateTime? dateFrom, DateTime? dateTo,
            IEnumerable<Guid> transports)
        {
            using (ZlatmetContext context = new ZlatmetContext())
            {
                object[] parameters =
                {
                    new SqlParameter
                    {
                        ParameterName = "@DateFrom",
                        SqlDbType = SqlDbType.Date,
                        Value = dateFrom
                    },
                    new SqlParameter
                    {
                        ParameterName = "@DateTo",
                        SqlDbType = SqlDbType.Date,
                        Value = dateTo
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Transports",
                        Value = string.Join(",", transports.Select(x => "'" + x.ToString() + "'").ToList())
                    }
                };

                string query = string.Format("ReportAutoTransport {0}",
                    string.Join(",", parameters.OfType<SqlParameter>().Select(x => x.ParameterName)));

                List<ReportAutoTransportData> data =
                    context.Database.SqlQuery<ReportAutoTransportData>(query, parameters).ToList();

                return data;
            }
        }

    }
}