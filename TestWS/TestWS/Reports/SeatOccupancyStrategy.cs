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
    public class SeatOccupancyStrategy : ReportBaseStrategy<SeatOccupancyModel>
    {
        public SeatOccupancyStrategy(IMapper mapper) : base(mapper) { }

        protected override SeatOccupancyModel GetDataModel()
        {
            var parameters = new[]
            {
                new SqlParameter("@startDate", (DateTime)Parameters[BaseReportsFormConstants.DateFrom]),
                new SqlParameter("@endDate", (DateTime)Parameters[BaseReportsFormConstants.DateTo])
            };
            var reportRows = DatabaseUtil.Execute<SeatOccupancyRow>("SeatOccupancy", parameters);
            return new SeatOccupancyModel() { Rows = reportRows };
        }

        protected override string InternalGetDownloadFileName()
        {
            return "SeatOccupancyReport";
        }

        protected override string InternalGetTemplateFileName()
        {
            return "SeatOccupancyReport.xlsx";
        }

        protected override void ProcessWorkbook(IWorkbook workbook, SeatOccupancyModel model)
        {
            var sheet = workbook.GetSheetAt(0);
            var rowIndex = 1;

            foreach (var row in model.Rows)
            {
                var documentRow = sheet.CreateRow(rowIndex);
                documentRow.CreateCell(SummaryColumns.Timeslot).SetCellValue(row.Timeslot);
                documentRow.CreateCell(SummaryColumns.MovieName).SetCellValue(row.MovieName);
                documentRow.CreateCell(SummaryColumns.SeatCount).SetCellValue(row.SeatCount);
                documentRow.CreateCell(SummaryColumns.SoldTickets).SetCellValue(row.SoldTickets);                
                rowIndex++;
            }

            var type = typeof(SummaryColumns);
            var columns = type.GetFields();
            foreach (var column in columns)
                sheet.AutoSizeColumn((int)column.GetValue(null));
        }

        private static class SummaryColumns
        {
            public const int Timeslot = 0;
            public const int MovieName = 1;
            public const int SeatCount = 2;
            public const int SoldTickets = 3;            
        }
    }
}