using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                //return View("Not_Found", id);
                return RedirectToRoute("NotFound", new {id});
                //return RedirectToRoute("Not", new { id });
            }

            return View(new DinnerFormViewModel(dinner));
        }

        // POST: /Dinners/Edit/1

        [HttpPost]
        public ActionResult Edit(int id, DinnerFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dinner = dnRepository.GetDinner(id);

                dinner.Address = viewModel.Address;
                dinner.ContactPhone = viewModel.ContactPhone;
                //dinner.DinnerID = viewModel.DinnerID;
                dinner.Description = viewModel.Description;
                dinner.HostedBy = viewModel.HostedBy;
                dinner.EventDate = viewModel.EventDate;
                dinner.Country = viewModel.Country;
                dinner.Longtitude = viewModel.Longtitude;
                dinner.Latitude = viewModel.Latitude;
                dinner.Title = viewModel.Title;

                dnRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
          
            return View(viewModel);

        }

        private IView DinnerFormViewModel(Dinner dinner)
        {
            throw new NotImplementedException();
        }

        
        // GET: /Dinners/Create
        public ActionResult Create()
        {
            //DinnerFormViewModel viewModel
            var dinner = new Dinner()
                {
                    EventDate = DateTime.Now.AddDays(7)
                };
            var viewModel = new DinnerFormViewModel(dinner);
            return View(viewModel);
        }

        // POST: /Dinners/Create
        
        [HttpPost]
        public ActionResult Create(DinnerFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dinner = new Dinner();

                dinner.Address = viewModel.Address;
                dinner.ContactPhone = viewModel.ContactPhone;
                //dinner.DinnerID = viewModel.DinnerID;
                dinner.Description = viewModel.Description;
                dinner.HostedBy = viewModel.HostedBy;
                dinner.EventDate = viewModel.EventDate;
                dinner.Country = viewModel.Country;
                dinner.Longtitude = viewModel.Longtitude;
                dinner.Latitude = viewModel.Latitude;
                dinner.Title = viewModel.Title;
                dnRepository.Add(dinner);
                dnRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            return View(viewModel);


            //var dinner = new Dinner();
            ////  ---- Returns null exception error here if dinner is null
            //if (TryUpdateModel(dinner)) // <---
            //{
            //    dnRepository.Add(dinner);
            //    dnRepository.Save();
            //    return RedirectToAction("Details", new {id = dinner.DinnerID});
            //}
            //return View(dinner);
        }
        

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
        public ActionResult DeletePost(int id)
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
            return View(id);
        }


        DinnerRepository dnRepository = new DinnerRepository();
    }
}
