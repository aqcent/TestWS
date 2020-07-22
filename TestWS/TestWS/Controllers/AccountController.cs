using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWS.Models;

namespace TestWS.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account        

        [HttpGet]
        public ActionResult LogIn()
        {
            var myLogin = new LogIn();         

            return View("~/Views/login.cshtml", myLogin);
        }

        [HttpPost]
        public ActionResult LogIn(LogIn loginResoult)
        {
            //var myLogin = new LogIn();
            if (!ModelState.IsValid)
            {
                return View("~/Views/login.cshtml", loginResoult);
            }
            return View("~/Views/LoginResoult.cshtml", loginResoult);
        }
    }
}