using Analogueweb.Mvc.Utilities;
using DataManager.Models.CarraraSQL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New State";
            return View("Manage", new State());
        }

        [HttpPost]
        public ActionResult Add(State model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.States.Add(model);
                DataContext.SaveChanges();
                HttpContext.Cache.Remove("StatesSelectList");
                HttpContext.Cache.Remove("StatesAbbrSelectList");
                TempData["Page.Title"] = "The State Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The State Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            State model = CarraraSQL.States.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate State with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.StateName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(State model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                State entity = CarraraSQL.States.Find(model.StateID);
                if (entity.RouteMileages.Any())
                {
                    Page.Title = "The State Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the State to Inactive if you do not intend to use this State again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.States.Attach(model);
                    CarraraSQL.States.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("StatesSelectList");
                    HttpContext.Cache.Remove("StatesAbbrSelectList");
                    TempData["Page.Title"] = "The State Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The State Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("StatesSelectList");
                HttpContext.Cache.Remove("StatesAbbrSelectList");
                TempData["Page.Title"] = "The State Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The State Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<State> model = CarraraSQL.States;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "abbreviation":
                    model = asc ? model.OrderBy(x => x.StateAbbreviation) : model.OrderByDescending(x => x.StateAbbreviation);
                    break;
                case "name":
                    model = asc ? model.OrderBy(x => x.StateName) : model.OrderByDescending(x => x.StateName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.StateID) : model.OrderByDescending(x => x.StateID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage States";
            page = page ?? 1;
            show = show ?? 40;
            int count = model.Count() == 0 ? 1 : model.Count();
            if (show.Value > count || show == 0)
            {
                show = count;
                page = 1;
            }
            int? max = (count / show) + 1;
            if (page > max)
            {
                page = max;
            }
            return View(model.ToPagedList(page.Value, show.Value));
        }
        #endregion
    }
}