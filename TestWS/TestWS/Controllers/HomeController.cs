using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Agent;
using TestWS.Models;
using TestWS.Reports;

namespace TestWS.Controllers
{
    public class HomeController : Controller
    {
        private IMapper Mapper { get; set; }

        public HomeController(IMapper mapper)
        {
            Mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var warm = new CacheWarmingUpAgent();
            warm.Run();

            ViewBag.Message = "Your application description page."; 

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}