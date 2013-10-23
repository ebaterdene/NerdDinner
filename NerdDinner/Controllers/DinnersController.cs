using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        private readonly IDinnerRepository _repository;

        public DinnersController(IDinnerRepository repository)
        {
            _repository = repository;
        }

        // GET: /Dinners/
        //      /Dinners/Page/1

        /* Index: Version 2 */
        public ActionResult Index(int page = 0)
        {
            const int pageSize = 10;
            var upcomingDinners = _repository.FindUpcomingDinners().ToList();
            var paginatedDinners = upcomingDinners.OrderBy(d => d.EventDate)
                                                  .Skip(page * pageSize)
                                                  .Take(pageSize)
                                                  .ToList();
            return View(paginatedDinners);
        }
       
        // ---- Details
        public ActionResult Details(int id)
        {
            Dinner dinner = _repository.GetDinner(id);

            if (dinner == null)
            {
                return RedirectToAction("NotFound", new { id });
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
            var dinner = _repository.GetDinner(id);

            if (dinner == null)
            {
                return RedirectToAction("NotFound", new { id });
            }

            return View(new DinnerFormViewModel(dinner));
        }

        // POST: /Dinners/Edit/1

        [HttpPost]
        public ActionResult Edit(DinnerFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dinner = _repository.GetDinner(viewModel.DinnerID);

                dinner.Address = viewModel.Address;
                dinner.ContactPhone = viewModel.ContactPhone;
                dinner.Country = viewModel.Country;
                dinner.Description = viewModel.Description;
                dinner.EventDate = viewModel.EventDate;
                dinner.HostedBy = viewModel.HostedBy;
                dinner.Latitude = viewModel.Latitude;
                dinner.Longtitude = viewModel.Longtitude;
                dinner.Title = viewModel.Title;

                _repository.Save();
                return RedirectToAction("Details", new { id = viewModel.DinnerID });
            }
          
            return View(viewModel);

        }
        
        // GET: /Dinners/Create
        public ActionResult Create()
        {
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
                dinner.Description = viewModel.Description;
                dinner.HostedBy = viewModel.HostedBy;
                dinner.EventDate = viewModel.EventDate;
                dinner.Country = viewModel.Country;
                dinner.Longtitude = viewModel.Longtitude;
                dinner.Latitude = viewModel.Latitude;
                dinner.Title = viewModel.Title;

                _repository.Add(dinner);
                _repository.Save();
                
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }

            return View(viewModel); 

        }
        

        // HTTP GET: /Dinners/Delete/1

        public ActionResult Delete(int id)
        {
            var dinner = _repository.GetDinner(id);
            if (dinner == null)
                return RedirectToAction("NotFound", new { id });
            else
                //returns a ViewResult
                return View(dinner);
        }

        // HTTP POST: /Dinners/Delete/1

        [HttpPost]
        public ActionResult DeletePost(int id)
        {

            var dinner = _repository.GetDinner(id);
            if (dinner == null)
            {
                return RedirectToAction("NotFound", new { id });
            }

            _repository.Delete(dinner);
            _repository.Save();

            return View("Deleted");
        }

        [HttpGet]
        public ViewResult NotFound(int id)
        {
            return View(id);
        }
    }
}
