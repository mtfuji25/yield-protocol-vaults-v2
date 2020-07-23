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
    public class RoutesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Route";
            return View("Manage", new Models.CarraraSQL.Route());
        }

        [HttpPost]
        public ActionResult Add(Models.CarraraSQL.Route model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Routes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("RoutesSelectList");
                TempData["Page.Title"] = "The Route Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Route Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Models.CarraraSQL.Route model = CarraraSQL.Routes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Route with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.RouteName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Models.CarraraSQL.Route model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Models.CarraraSQL.Route entity = CarraraSQL.Routes.Find(model.RouteID);
                if (entity.Jobs.Any() ||
                    entity.Loads.Any() ||
                    entity.RouteMileages.Any())
                {
                    Page.Title = "The Route Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Route to Inactive if you do not intend to use this Route again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Routes.Attach(model);
                    CarraraSQL.Routes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("RoutesSelectList");
                    TempData["Page.Title"] = "The Route Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Route Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (model.Action == "Mileage")
            {
                return RedirectToAction("Index", "RouteMileage", new { guid = model.RouteID });
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Route Was Updated Successfully";
                HttpContext.Cache.Remove("RoutesSelectList");
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Route Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, bool? cement, string order, int? page, int? show, string sort)
        {
            IEnumerable<Models.CarraraSQL.Route> model = CarraraSQL.Routes;
            // Filter by active/inactive
            if (active.HasValue)
            {
                model = model.Where(x => x.IsActive == active.Value);
            }
            if (!cement.HasValue || (cement.HasValue && cement.Value == true))
            {
                model = model.Where(x => x.IsCementHaulingRoute);
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "active":
                    model = asc ? model.OrderBy(x => x.IsActive) : model.OrderByDescending(x => x.IsActive);
                    break;
                case "cement-hauling-route":
                    model = asc ? model.OrderBy(x => x.IsCementHaulingRoute) : model.OrderByDescending(x => x.IsCementHaulingRoute);
                    break;
                case "name":
                    model = asc ? model.OrderBy(x => x.RouteName) : model.OrderByDescending(x => x.RouteName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.RouteID) : model.OrderByDescending(x => x.RouteID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Routes";
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