using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class MyDinnersController : Controller
    {
        //
        // GET: /MyDinners/

        /* Version 1
        public ActionResult Index()
        {
            ViewBag.Message = "Main Index page for My Dinner";


            return View();
        }
        */

        /* Version 2
         
        public void Index()
        {
            //Response.Write("<h1>Coming Soon: Dinners</h1>");
            var dinners = dnRepository.FindUpcomingDinners().ToList();

        }


        public void My_Dinner_Details(int id)
        {
            //Response.Write("<h1>_Dinners Details " + id + "</h1>");
            Dinner dinner = dnRepository.GetDinner(id);

        }
        */
        /* Version 3
         */
        public ActionResult Index()
        {
            //Response.Write("<h1>Coming Soon: Dinners</h1>");
            var dinners = dnRepository.FindUpcomingDinners().ToList();
            return View("Index", dinners);

        }
        public void View_All_Dinners()
        {
            //Response.Write("<h1>Coming Soon: Dinners</h1>");
            var dinners = dnRepository.FindUpcomingDinners().ToList();
            //var dinners = nde.Dinners.Select()
            //return View(dinners);

        }

        public ActionResult View_One_Dinner()
        {
            //Response.Write("<h1>Coming Soon: Dinners</h1>");
            //var dinners = dnRepository.FindUpcomingDinners().ToList();
            //NerDinnerEntities nde;
            //var dinners = nde.Dinners.Select()
            return View(model: GetDinnerById(1));

        }

        public Dinner GetDinnerById(int id)
        {
            using (var dbEntities = new NerdDinners())
            {
                return dbEntities.Dinners.First(d => d.DinnerID == id);
            }
        }


        public ActionResult My_Dinner_Details(int id)
        {
            //Response.Write("<h1>_Dinners Details " + id + "</h1>");
            Dinner dinner = dnRepository.GetDinner(id);
            if (dinner == null)
                return View("Not_Found");
            else
                return View( dinner);
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


        //Vars

        private DinnerRepository dnRepository = new DinnerRepository();


    }
}
