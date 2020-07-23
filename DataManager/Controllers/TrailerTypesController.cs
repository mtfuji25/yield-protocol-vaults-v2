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
    public class TrailerTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Trailer Type";
            return View("Manage", new TrailerType());
        }

        [HttpPost]
        public ActionResult Add(TrailerType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.TrailerTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("TrailerTypesSelectList");
                TempData["Page.Title"] = "The Trailer Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Trailer Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            TrailerType model = CarraraSQL.TrailerTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Trailer Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.TrailerTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(TrailerType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                TrailerType entity = CarraraSQL.TrailerTypes.Find(model.TrailerTypeID);
                if (entity.Loads.Any() ||
                    entity.Vehicles.Any())
                {
                    Page.Title = "The Trailer Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Trailer Type to Inactive if you do not intend to use this Trailer Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.TrailerTypes.Attach(model);
                    CarraraSQL.TrailerTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("TrailerTypesSelectList");
                    TempData["Page.Title"] = "The Trailer Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Trailer Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("TrailerTypesSelectList");
                TempData["Page.Title"] = "The Trailer Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Trailer Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<TrailerType> model = CarraraSQL.TrailerTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.TrailerTypeName) : model.OrderByDescending(x => x.TrailerTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.TrailerTypeID) : model.OrderByDescending(x => x.TrailerTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Trailer Types";
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