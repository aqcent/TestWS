using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Models.Reports;
using TestWS.Reports;

namespace TestWS.Factories
{
    public static class ReportsFactory
    {
        public static string GetReportFormView(ReportType type)
        {
            switch (type)
            {
                case ReportType.PotentialRealProfit:
                    return "~/Views/TicketsAdmin/Reports/PotentialRealProfitForm.cshtml";                    
                case ReportType.SeatOccupancy:
                    return "~/Views/TicketsAdmin/Reports/SeatOccupancyForm.cshtml";                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static BaseReportForm GetReportFormModel(ControllerContext context, ReportType type, IMapper mapper)
        {
            switch (type)
            {
                case ReportType.PotentialRealProfit:
                    return mapper.Map<PotentialRealProfitReportForm>(context);
                case ReportType.SeatOccupancy:
                    return mapper.Map<SeatOccupancyReportForm>(context);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IReportBuilder GetReportStrategy(BaseReportForm form, IMapper mapper)
        {
            switch (form.ReportType)
            {
                case ReportType.PotentialRealProfit:
                    {
                        var formModel = (PotentialRealProfitReportForm) form;
                        var strategy = new PotentialRealProfitStrategy(mapper)
                        {
                            Parameters = formModel.GetParameters()
                        };
                        return strategy;
                    }
                    
                case ReportType.SeatOccupancy:
                    {
                        var formModel = (SeatOccupancyReportForm) form;
                        var strategy = new SeatOccupancyStrategy(mapper)
                        {
                            Parameters = formModel.GetParameters()
                        };
                        return strategy;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
    }
}