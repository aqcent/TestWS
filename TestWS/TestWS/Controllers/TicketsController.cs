using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TestWS.Attributes;
using TestWS.Extensions;
using TestWS.Managers;
using TestWS.Models.Tickets;
using TestWS.Services;

namespace TestWS.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly ICacheManager _cacheManager;

        public TicketsController(ITicketService ticketService, ICacheManager cacheManager)
        {
            _ticketService = ticketService;
            _cacheManager = cacheManager;
        }

        public ActionResult GetMovies()
        {
            var cacheKey = "Tickets_GetMovies";


            var allMovies = _cacheManager.CacheResult(() => _ticketService.GetFullMoviesInfo(), cacheKey);
            return View("~/Views/Tickets/MoviesList.cshtml", allMovies);
        }

        public ActionResult GetHallInfo(int timeSlotID)
        {
            var timeslot = _ticketService.GetTimeSlotByID(timeSlotID);
            var currentTarrif = _ticketService.GetTariffByID(timeslot.TariffID);

            var b = Request.UrlReferrer;
            var a = HttpContext.Request.UrlReferrer;

            var model = new HallInfo
            {
                CurrentTariff = currentTarrif,
                CurrentTimeslotID = timeSlotID,
                RequestedSeats = timeslot.RequestedSeats
            };

            return View("~/Views/Tickets/HallInfo.cshtml", model);
        }

        public string GetHallInfoWithLayout(int timeSlotID)
        {
            var timeslot = _ticketService.GetTimeSlotByID(timeSlotID);
            var currentHall = _ticketService.GetHallByID(timeslot.HallID);
            var model = new
            {
                RequestedSeats = timeslot.RequestedSeats,
                HallLayout = currentHall.Layout
            };
            return JsonConvert.SerializeObject(model);
        }

        public string ProcessRequest(SeatsProcessRequest request)
        {
            var requestProcessingResult = _ticketService.AddRequestedSeatsToTimeslot(request);
            return JsonConvert.SerializeObject(new
            {
                requestResult = requestProcessingResult
            });
        }


        public string ProcessSearchRequest(string searchRequest)
        {
            var cacheKey = string.Format("Tickets_ProcessSearchRequest_searchRequest:{0}", searchRequest);            

            var cacheResult = _cacheManager.CacheResult(() =>
            {
                var resoultList = _ticketService.SearchFilmByName(searchRequest);
                return JsonConvert.SerializeObject(resoultList);
            }, 
            cacheKey);
            
            return cacheResult;
        }

    }
}