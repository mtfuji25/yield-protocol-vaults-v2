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
    public class RouteMileageController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Route Mileage";
            return View("Manage", new RouteMileage());
        }

        [HttpPost]
        public ActionResult Add(RouteMileage model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.RouteMileages.Add(model);
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Route Mileage Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Route Mileage Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            RouteMileage model = CarraraSQL.RouteMileages.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Route Mileage with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit Mileage for " + model.State.StateAbbreviation + "-" + model.Route.RouteName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(RouteMileage model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                try
                {
                    CarraraSQL.RouteMileages.Attach(model);
                    CarraraSQL.RouteMileages.Remove(model);
                    CarraraSQL.SaveChanges();
                    TempData["Page.Title"] = "The Route Mileage Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Route Mileage Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Route Mileage Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Route Mileage Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(int? guid, string order, int? page, int? show, int? state, string sort)
        {
            IEnumerable<RouteMileage> model = CarraraSQL.RouteMileages;
            // Filter by active/inactive
            if (state.HasValue)
            {
                model = model.Where(x => x.StateID == state.Value);
            }
            if (guid.HasValue)
            {
                model = model.Where(x => x.RouteID == guid.Value);
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "state":
                    model = asc ? model.OrderBy(x => x.State.StateName) : model.OrderByDescending(x => x.State.StateName);
                    break;
                case "route":
                    model = asc ? model.OrderBy(x => x.Route.RouteName) : model.OrderByDescending(x => x.Route.RouteName);
                    break;
                case "mileage":
                    model = asc ? model.OrderBy(x => x.Mileage) : model.OrderByDescending(x => x.Mileage);
                    break;
                case "id":
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.RouteMileageID) : model.OrderByDescending(x => x.RouteMileageID);
                    break;
            }
            // Return Results
            if (guid.HasValue)
            {
                Models.CarraraSQL.Route route = CarraraSQL.Routes.Find(guid.Value);
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Route Mileages for the " + route.RouteName + " Route";
            }
            else
            {
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Route Mileages";
            }
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