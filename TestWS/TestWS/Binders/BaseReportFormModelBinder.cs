using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Factories;
using TestWS.Models.Reports;

namespace TestWS.Binders
{
    public class BaseReportFormModelBinder : DefaultModelBinder
    {
        private readonly IMapper _mapper;

        public BaseReportFormModelBinder()
        {
            _mapper = DependencyResolver.Current.GetService<IMapper>();
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!(base.BindModel(controllerContext, bindingContext) is BaseReportForm model))
                return null;

            return ReportsFactory.GetReportFormModel(controllerContext, model.ReportType, _mapper);            
        }
    }
}