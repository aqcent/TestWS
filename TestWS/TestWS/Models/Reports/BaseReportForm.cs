using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWS.Factories;

namespace TestWS.Models.Reports
{
    public class BaseReportForm
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ReportType ReportType { get; set; }
        public virtual Dictionary<string, object> GetParameters()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add(BaseReportsFormConstants.DateFrom, DateFrom);
            parameters.Add(BaseReportsFormConstants.DateTo, DateTo);
            return parameters;
        }
    }

    public static class BaseReportsFormConstants
    {
        public static string DateFrom => "DateFrom";
        public static string DateTo => "DateTo";
        public static string ReportType => "ReportType";
    }
}