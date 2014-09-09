﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Dapper;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Service;
using Zlatmet2.Views.Reports;

namespace Zlatmet2.ViewModels.Reports
{
    public sealed class ReportNomenclatureViewModel : BaseReportViewModel
    {
        private readonly Template _template;

        private DateTime _dateFrom;

        private DateTime _dateTo;

        public ReportNomenclatureViewModel(LayoutDocument layout, Guid id, object optional = null)
            : base(layout, typeof(ReportNomenclatureView), id)
        {
            Title = "Обороты за период";

            Id = Guid.NewGuid();

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;

            _template = MainStorage.Instance.TemplatesRepository.GetByName(ReportName);

            Report = new StiReport();
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (value.Equals(_dateFrom))
                    return;
                _dateFrom = value;
                RaisePropertyChanged("DateFrom");
            }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                if (value.Equals(_dateTo))
                    return;
                _dateTo = value;
                RaisePropertyChanged("DateTo");
            }
        }


        public override string ReportName
        {
            get { return "Обороты за период"; }
        }

        protected override void PrepareReport()
        {
            if (_template == null)
            {
                MessageBox.Show(string.Format("Отсутствует шаблон \"{0}\"", ReportName), MainStorage.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<ReportData> reportData = PrepareData();

            Report = new StiReport();
            Report.Load(_template.Data);

            //Report.Dictionary.Variables["ReportDate"].Value = Date.ToShortDateString();

            Report.RegBusinessObject("Data", reportData);

            Report.Compile();
            Report.Render(false);
        }

        private List<ReportData> PrepareData()
        {
            List<ReportData> reportData = new List<ReportData>();

            using (IDbConnection connection = MainStorage.Instance.ConnectionFactory.Create())
            {
                var p = new DynamicParameters();
                p.Add("@DateFrom", DateFrom, DbType.Date);
                p.Add("@DateTo", DateTo, DbType.Date);

                List<Dto> dtos =
                    connection.Query<Dto>("usp_ReportNomenclature", p, commandType: CommandType.StoredProcedure)
                        .ToList();
                for (int i = 0; i < dtos.Count; i++)
                {
                    Dto dto = dtos[i];
                    reportData.Add(new ReportData
                    {
                        Number = i + 1,
                        Name = dto.Nomenclature,
                        Purchased = dto.Purchased,
                        Sold = dto.Sold
                    });
                }
            }

            return reportData;
        }

        private class ReportData
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public double Purchased { get; set; }
            public double Sold { get; set; }
        }

        private class Dto
        {
            public string Nomenclature { get; set; }
            public double Purchased { get; set; }
            public double Sold { get; set; }
        }
    }
}
