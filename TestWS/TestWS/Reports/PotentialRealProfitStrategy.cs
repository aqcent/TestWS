using AutoMapper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestWS.Models.Reports;

namespace TestWS.Reports
{
    public class PotentialRealProfitStrategy : ReportBaseStrategy<PotentialRealProfitModel>
    {
        public PotentialRealProfitStrategy(IMapper mapper) : base(mapper) { }
        protected override PotentialRealProfitModel GetDataModel()
        {
            var parameters = new[]
            {
                new SqlParameter("@StartDate", (DateTime)Parameters[BaseReportsFormConstants.DateFrom]),
                new SqlParameter("@EndDate", (DateTime)Parameters[BaseReportsFormConstants.DateTo])
            };

            var reportRows = DatabaseUtil.Execute<PotentialRealProfitRow>("PotentialRealProfit", parameters);

            return new PotentialRealProfitModel() { Rows = reportRows };
        }

        protected override string InternalGetDownloadFileName()
        {
            return "PotentialRealProfitReport";
        }

        protected override string InternalGetTemplateFileName()
        {
            return "PotentialRealProfitReport.xlsx";
        }

        protected override void ProcessWorkbook(IWorkbook workbook, PotentialRealProfitModel model)
        {
            var sheet = workbook.GetSheetAt(0);
            var rowIndex = 1;

            foreach (var row in model.Rows)
            {
                var documentRow = sheet.CreateRow(rowIndex);
                documentRow.CreateCell(SummaryColumns.MovieName).SetCellValue(row.Name);
                documentRow.CreateCell(SummaryColumns.GarantedProfit).SetCellValue(row.RealProfit);
                documentRow.CreateCell(SummaryColumns.PurchasedTickets).SetCellValue(row.PurchasedTickets);
                documentRow.CreateCell(SummaryColumns.PotentialProfit).SetCellValue(row.PotentialProfit);
                documentRow.CreateCell(SummaryColumns.ReservedTickets).SetCellValue(row.ReservedTickets);
                rowIndex++;
            }

            sheet.AutoSizeColumn(SummaryColumns.MovieName);
            sheet.AutoSizeColumn(SummaryColumns.GarantedProfit);
            sheet.AutoSizeColumn(SummaryColumns.PurchasedTickets);
            sheet.AutoSizeColumn(SummaryColumns.PotentialProfit);
            sheet.AutoSizeColumn(SummaryColumns.ReservedTickets);
        }

        private static class SummaryColumns
        {
            public const int MovieName = 0;
            public const int GarantedProfit = 1;
            public const int PurchasedTickets = 2;
            public const int PotentialProfit = 3;
            public const int ReservedTickets = 4;
        }
    }
}