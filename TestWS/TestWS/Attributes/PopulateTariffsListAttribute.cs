using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Services;

namespace TestWS.Attributes
{
    public class PopulateTariffsListAttribute : ActionFilterAttribute
    {
        public ITicketService TicketService { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            filterContext.Controller.ViewData["TariffsList"] = TicketService.GetAllTariffs();
            base.OnActionExecuting(filterContext);
        }
    }
}