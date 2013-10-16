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

        // ---- Index
        
        public ActionResult Index()
        {
            var dinners = dnRepository.FindUpcomingDinners().ToList();

            return View(dinners);
        }

        // ---- Details

        public ActionResult Details(int id)
        {
            var dinner = dnRepository.GetDinner(id);

            ////Error Happens when I 
            //if (id.GetType() != null)
            //{
            //    id = 0;
            //}

            if (dinner == null)
            {
                return RedirectToRoute("NotFound", new { id });
            }
            else
            {
                return View(dinner);
            }
        }


        // ---- Edit

        // GET: /Dinners/Edit/1

        public ActionResult Edit(int id)
        {
            var dinner = dnRepository.GetDinner(id);

            if (dinner == null)
            {
                return RedirectToRoute("NotFound", new { id });
            }

            var countries = new[] { "USA", "Afghanistan", "Albania", "Zimbabwe" };
            ViewData["countries"] = new SelectList(countries, dinner.Country);

            return View(dinner);
        }

        // POST: /Dinners/Edit/1

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formValues)
        {

            var dinner = dnRepository.GetDinner(id);
            

            //----- Argument Null Exception found.

            if (TryUpdateModel(dinner))
            {
                dnRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }

            return View(dinner);
        }

        

        // ---- Create
        
        // GET: /Dinners/Create
        public ActionResult Create()
        {
            var dinner = new Dinner()
                {
                    EventDate = DateTime.Now.AddDays(7)
                };
            return View(dinner);
        }

        // POST: /Dinners/Create
        
        [HttpPost]
        public ActionResult Create(FormCollection formValues)
        {
            var dinner = new Dinner();
            //  ---- Returns null exception error here if dinner is null
            if (TryUpdateModel(dinner)) // <---
            {
                dnRepository.Add(dinner);
                dnRepository.Save();
                return RedirectToAction("Details", new {id = dinner.DinnerID});
            }
            return View(dinner);
        }
        

        
        /*
        [HttpPost]
        public ActionResult Create(Dinner dinner)
        {
            if (ModelState.IsValid)
            {
                dinner.HostedBy = "SomeUser";
                dnRepository.Add(dinner);
                dnRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }

            return View(dinner);
        }
        */
        // ---- Delete

        // HTTP GET: /Dinners/Delete/1

        public ActionResult Delete(int id)
        {
            var dinner = dnRepository.GetDinner(id);
            if (dinner == null)
                return RedirectToRoute("NotFound", new {id} );
            else
                return View(dinner);
        }

        // HTTP POST: /Dinners/Delete/1

        [HttpPost]
        public ActionResult Delete(int id, string confirmButton)
        {

            var dinner = dnRepository.GetDinner(id);
            if (dinner == null)
            {
                return RedirectToRoute("NotFound", new { id });
            }

            dnRepository.Delete(dinner);
            dnRepository.Save();

            return View("Deleted");
        }

        [HttpGet]
        public ViewResult NotFound(int id)
        {
            return View("Not_Found", id);
        }


        DinnerRepository dnRepository = new DinnerRepository();
    }
}
