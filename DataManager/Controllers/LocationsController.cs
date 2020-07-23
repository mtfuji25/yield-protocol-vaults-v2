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
    public class LocationsController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Location";
            return View("Manage", new Location());
        }

        [HttpPost]
        public ActionResult Add(Location model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Locations.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("LocationsSelectList");
                TempData["Page.Title"] = "The Location Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Location Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Location model = CarraraSQL.Locations.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Location with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.LocationName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Location model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Location entity = CarraraSQL.Locations.Find(model.LocationID);
                if (entity.Employees.Any() ||
                    entity.RediMixOrders.Any() ||
                    entity.Vehicles.Any())
                {
                    Page.Title = "The Location Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Location to Inactive if you do not intend to use this Location again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Locations.Attach(model);
                    CarraraSQL.Locations.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("LocationsSelectList");
                    TempData["Page.Title"] = "The Location Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Location Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("LocationsSelectList");
                TempData["Page.Title"] = "The Location Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Location Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<Location> model = CarraraSQL.Locations;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.LocationName) : model.OrderByDescending(x => x.LocationName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.LocationID) : model.OrderByDescending(x => x.LocationID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Locations";
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