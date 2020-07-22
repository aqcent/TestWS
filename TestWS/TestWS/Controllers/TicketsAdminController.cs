using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Attributes;
using TestWS.Binders;
using TestWS.Factories;
using TestWS.Models.Reports;
using TestWS.Models.Tickets;
using TestWS.Services;

namespace TestWS.Controllers
{
    public class TicketsAdminController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public TicketsAdminController(ITicketService ticketService, IMapper mapper)
        {
            _ticketService = ticketService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult BuildReport([ModelBinder(typeof(BaseReportFormModelBinder))]BaseReportForm form)
        {
            var reportStrategy = ReportsFactory.GetReportStrategy(form, _mapper);
            var reportLink = reportStrategy.BuildReport();
            return View("~/Views/TicketsAdmin/Reports/DownloadReport.cshtml", model: reportLink);
        }

        [HttpGet]
        public ActionResult Reports()
        {
            return View("~/Views/TicketsAdmin/Reports/Reports.cshtml");
        }

        public ActionResult GetReportForm(ReportType type)
        {
            var currentView = ReportsFactory.GetReportFormView(type);
            return PartialView(currentView);
        }

        public ActionResult GetMoviesList()
        {
            var movies = _ticketService.GetAllMovies();
            return View("~/Views/TicketsAdmin/MoviesList.cshtml", movies);
        }
        [HttpGet]
        public ActionResult EditMovie(int movieID)
        {
            var movie = _ticketService.GetMovieByID(movieID);
            return View("~/Views/TicketsAdmin/EditMovie.cshtml", movie);
        }
        [HttpPost]
        public ActionResult EditMovie(Movie updatedMovie)
        {
            var updateResoult = _ticketService.UpdateMovie(updatedMovie);
            if (updateResoult)
                return RedirectToAction("GetMoviesList");

            return Content("Update failed.");
        }
        [HttpGet]
        public ActionResult AddMovie()
        {
            return View("~/Views/TicketsAdmin/AddMovie.cshtml");
        }
        [HttpPost]
        public ActionResult AddMovie(Movie newMovie)
        {
            var creationResoult = _ticketService.CreateMovie(newMovie);
            if (creationResoult)
                return RedirectToAction("GetMoviesList");

            return Content("Update failed.");
        }
        public ActionResult RemoveMovie(int movieID)
        {
            var removeResoult = _ticketService.RemoveMovie(movieID);
            if (removeResoult)
                return RedirectToAction("GetMoviesList");
            return Content("Update failed.");
        }

        public ActionResult RemoveTimeSlot(int timeslotID)
        {
            var removeResult = _ticketService.RemoveTimeSlot(timeslotID);
            if (removeResult)
                return RedirectToAction("GetTimeSlotsList");
            return Content("Update failed.");
        }

        public ActionResult RemoveTariff(int tariffID)
        {
            var removeResult = _ticketService.RemoveTariff(tariffID);
            if (removeResult)
                return RedirectToAction("GetTariffsList");
            return Content("Update failed.");
        }

        public ActionResult RemoveHall(int hallID)
        {
            var removeResult = _ticketService.RemoveHall(hallID);
            if (removeResult)
                return RedirectToAction("GetHallsList");
            return Content("Update failed.");
        }

        public ActionResult GetHallsList()
        {
            var halls = _ticketService.GetAllHalls();
            return View("~/Views/TicketsAdmin/HallsList.cshtml", halls);
        }
        [HttpGet]
        public ActionResult EditHall(int hallID)
        {
            var hall = _ticketService.GetHallByID(hallID);
            return View("~/Views/TicketsAdmin/EditHall.cshtml", hall);
        }
        [HttpPost]
        public ActionResult EditHall(Hall updatedHall)
        {
            var updateResoult = _ticketService.UpdateHall(updatedHall);
            if (updateResoult)
                return RedirectToAction("GetHallsList");

            return Content("Update failed.");
        }

        [HttpGet]
        public ActionResult AddHall()
        {
            return View("~/Views/TicketsAdmin/AddHall.cshtml");
        }
        [HttpPost]
        public ActionResult AddHall(Hall newHall)
        {
            var creationResoult = _ticketService.CreateHall(newHall);
            if (creationResoult)
                return RedirectToAction("GetHallsList");

            return Content("Update failed.");
        }



        public ActionResult GetTariffsList()
        {
            var tariffs = _ticketService.GetAllTariffs();
            return View("~/Views/TicketsAdmin/TariffsList.cshtml", tariffs);
        }
        [HttpGet]
        public ActionResult EditTariff(int tariffID)
        {
            var tariff = _ticketService.GetTariffByID(tariffID);
            return View("~/Views/TicketsAdmin/EditTariff.cshtml", tariff);
        }
        [HttpPost]
        public ActionResult EditTariff(Tariff updatedTariff)
        {
            var updateResoult = _ticketService.UpdateTariff(updatedTariff);
            if (updateResoult)
                return RedirectToAction("GetTariffsList");
            return Content("Update failed.");
        }

        [HttpGet]
        public ActionResult AddTariff()
        {
            return View("~/Views/TicketsAdmin/AddTariff.cshtml");
        }
        [HttpPost]
        public ActionResult AddTariff(Tariff newTariff)
        {
            var creationResoult = _ticketService.CreateTariff(newTariff);
            if (creationResoult)
                return RedirectToAction("GetTariffsList");

            return Content("Update failed.");
        }



        public ActionResult GetTimeSlotsList()
        {
            var timeslots = _ticketService.GetAllTimeSlots();
            var resoultModel = ProcessTimeSlots(timeslots);
            return View("~/Views/TicketsAdmin/TimeSlotsList.cshtml", resoultModel);
        }

        public ActionResult GetTimeSlotsListByMovieID(int movieID)
        {
            var timeslots = _ticketService.GetTimeSlotsByMovieID(movieID);
            var resoultModel = ProcessTimeSlots(timeslots);
            return View("~/Views/TicketsAdmin/TimeSlotsList.cshtml", resoultModel);
        }

        private TimeSlotGridRow[] ProcessTimeSlots(TimeSlot[] timeslots)
        {
            var movies = _ticketService.GetAllMovies();
            var halls = _ticketService.GetAllHalls();
            var tariffs = _ticketService.GetAllTariffs();

            var resoultModel = new List<TimeSlotGridRow>();
            foreach (var timeslot in timeslots)
            {
                resoultModel.Add(new TimeSlotGridRow
                {
                    ID = timeslot.Id,
                    StartTime = timeslot.StartTime,
                    HallName = halls.First(x => x.Id == timeslot.HallID).Name,
                    MovieName = movies.First(x => x.Id == timeslot.MovieID).Name,
                    TariffName = tariffs.First(x => x.Id == timeslot.TariffID).Name
                });
            }

            return resoultModel.ToArray();
        }

        [HttpGet]
        [PopulateMoviesList, PopulateHallsList, PopulateTariffsList]
        public ActionResult EditTimeSlot(int timeslotID)
        {
            Session["page"] = Request.UrlReferrer.ToString();
            var timeslot = _ticketService.GetTimeSlotByID(timeslotID);
            return View("~/Views/TicketsAdmin/EditTimeSlot.cshtml", timeslot);
        }
        [HttpPost]
        public ActionResult EditTimeSlot(TimeSlot updatedTimeSlot)
        {
            var updateResoult = _ticketService.UpdateTimeSlot(updatedTimeSlot);
            if (updateResoult)
            {
                object refUrl = Session["page"];
                if (refUrl != null)
                    Response.Redirect((string)refUrl);
            }
            //return RedirectToAction("GetTimeSlotsList");
            //return Redirect(Request.UrlReferrer.ToString());
            return Content("Update failed.");
        }

        [HttpGet]
        [PopulateMoviesList, PopulateHallsList, PopulateTariffsList]
        public ActionResult AddTimeSlot()
        {

            return View("~/Views/TicketsAdmin/AddTimeSlot.cshtml", null);
        }
        [HttpPost]
        public ActionResult AddTimeSlot(TimeSlot newTimeSlot)
        {
            var creationResoult = _ticketService.CreateTimeSlot(newTimeSlot);
            if (creationResoult)
                return RedirectToAction("GetTimeSlotsList");
            return Content("Update failed.");
        }



        public ActionResult FindMovieByID(int id)
        {
            var movie = _ticketService.GetMovieByID(id);

            if (movie == null)
                return Content("Movie with such ID doesn't exist.");

            var modeljson = JsonConvert.SerializeObject(movie);
            return Content(modeljson, "application/json");
        }
        public ActionResult FindHallByID(int id)
        {
            var hall = _ticketService.GetHallByID(id);

            if (hall == null)
                return Content("Hall with such ID doesn't exist.");

            var modeljson = JsonConvert.SerializeObject(hall);
            return Content(modeljson, "application/json");
        }
        public ActionResult FindTariffByID(int id)
        {
            var tariff = _ticketService.GetTariffByID(id);

            if (tariff == null)
                return Content("Tariff with such ID doesn't exist.");

            var modeljson = JsonConvert.SerializeObject(tariff);
            return Content(modeljson, "application/json");
        }
        public ActionResult FindTimeSlotByID(int id)
        {
            var timeslot = _ticketService.GetTimeSlotByID(id);

            if (timeslot == null)
                return Content("TimeSlot with such ID doesn't exist.");

            var modeljson = JsonConvert.SerializeObject(timeslot);
            return Content(modeljson, "application/json");
        }
    }
}