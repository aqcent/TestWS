using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Models.Reports;

namespace TestWS.Profiles
{
    public class ControllerContextProfile : Profile
    {
        public ControllerContextProfile()
        {
            CreateMap<ControllerContext, BaseReportForm>()
                .ForMember(x => x.DateFrom, x => x.MapFrom(z => DateTime.Parse(z.HttpContext.Request.Form[BaseReportsFormConstants.DateFrom])))
                .ForMember(x => x.DateTo, x => x.MapFrom(z => DateTime.Parse(z.HttpContext.Request.Form[BaseReportsFormConstants.DateTo])))
                .ForMember(x => x.ReportType, x => x.MapFrom(z => Enum.Parse(typeof(ReportType), z.HttpContext.Request.Form[BaseReportsFormConstants.ReportType])));

            CreateMap<ControllerContext, PotentialRealProfitReportForm>()
                .IncludeBase<ControllerContext, BaseReportForm>();

            CreateMap<ControllerContext, SeatOccupancyReportForm>()
                .IncludeBase<ControllerContext, BaseReportForm>();
        }
    }
}