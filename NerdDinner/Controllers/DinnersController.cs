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


        // ---- Edit

        public ActionResult Edit(int id)
        {
            var dinner = dnRepository.GetDinner(id);

            if (dinner == null)
            {
                return View("Not_Found", id);
            }

            return View(dinner);
        }

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

        // ---- Update

        public void UpdateModel(Dinner dinner)
        {

            //-- Not sure if Request.Form works here.   ---

            dinner.Title = Request.Form["Title"];
            dinner.Description = Request.Form["Description"];
            dinner.EventDate = DateTime.Parse(Request.Form["EventDate"]);
            dinner.Address = Request.Form["Address"];
            dinner.Country = Request.Form["Country"];
            dinner.ContactPhone = Request.Form["ContactPhone"];
            dnRepository.Save();
        }

        // ---- Create
        
        public ActionResult Create()
        {
            var dinner = new Dinner()
                {
                    EventDate = DateTime.Now.AddDays(7)
                };
            return View(dinner);
        }


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

        // ---- Delete

        public ActionResult Delete(int id)
        {
            var dinner = dnRepository.GetDinner(id);
            if (dinner == null)
            {
                //-----View is not returning Not_Found
                return View("Not_Found", id);
            }
            else
                return View(dinner);
        }

        [HttpPost]
        public ActionResult Delete(int id, string confirmButton)
        {

            var dinner = dnRepository.GetDinner(id);
            if (dinner == null)
            {
                //-----View is not returning Not_Found
                return View("Not_Found");
            }
            dnRepository.Delete(dinner);
            dnRepository.Save();

            return View("Deleted");
        }


        DinnerRepository dnRepository = new DinnerRepository();
    }
}
