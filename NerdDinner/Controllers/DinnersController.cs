using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        //
        // GET: /Dinners/

        /*
        public ActionResult Index()
        {
            var dinners = dnRepository.FindUpcomingDinners().ToList();
            //var dinners = dinnerDepository.FindUpcomingDinners().ToList();

            return View("Index", dinners);

            //Response.Write("<h1>Coming Soon: Dinners</h1>");
        }
        */
        
        public ActionResult Index()
        {
            var dinners = dnRepository.FindUpcomingDinners().ToList();

            return View(dinners);
        }

        public ActionResult Details(int id)
        {
            var dinner = dnRepository.GetDinner(id);


            //Error Happens when I 
            if (id.GetType() != null)
            {
                id = 0;
            }

            if (dinner == null)
            {
                return View("Not_Found", id);
            }
            else
            {
                return View(dinner);
            }
        }

        public ActionResult Edit(int id)
        {
            var dinner = dnRepository.GetDinner(id);
            return View(dinner);
        }

        public void Delete(int id)
        {
            
        }
        
        public void Create()
        {
            
        }

        /*
        public void Details(int id)
        {
            Response.Write("<h1>Details DinnerID: "+ id + "</h1>");
        }
        */
        DinnerRepository dnRepository = new DinnerRepository();




    }
}
