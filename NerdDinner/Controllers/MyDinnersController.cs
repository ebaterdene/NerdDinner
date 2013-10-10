using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinner.Controllers
{
    public class MyDinnersController : Controller
    {
        //
        // GET: /MyDinners/

        public ActionResult Index()
        {
            ViewBag.Message = "Main Index page for My Dinner";
            return View();
        }


        public ActionResult Second_Page()
        {
            ViewBag.Message = "Second page for My Dinner";
            return View();
        }

        public ActionResult Fetch_Dinner_Data()
        {
            ViewBag.Message = "Fetching Dinner Data.";
            return View();
        }

        
    }
}
