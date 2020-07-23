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
    public class MixesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Mix";
            return View("Manage", new Mix());
        }

        [HttpPost]
        public ActionResult Add(Mix model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Mixes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MixesSelectList");
                TempData["Page.Title"] = "The Mix Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Mix Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Mix model = CarraraSQL.Mixes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Mix with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.MixName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Mix model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Mix entity = CarraraSQL.Mixes.Find(model.MixID);
                if (entity.Beds.Any() ||
                    entity.Pours.Any() ||
                    entity.RediMixOrders.Any())
                {
                    Page.Title = "The Mix Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Mix to Inactive if you do not intend to use this Mix again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Mixes.Attach(model);
                    CarraraSQL.Mixes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("MixesSelectList");
                    TempData["Page.Title"] = "The Mix Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Mix Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MixesSelectList");
                TempData["Page.Title"] = "The Mix Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Mix Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, string order, int? page, int? show, string sort)
        {
            IEnumerable<Mix> model = CarraraSQL.Mixes;
            // Filter by active/inactive
            if (active.HasValue)
            {
                model = model.Where(x => x.IsActive == active.Value);
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
                case "name":
                    model = asc ? model.OrderBy(x => x.MixName) : model.OrderByDescending(x => x.MixName);
                    break;
                case "description":
                    model = asc ? model.OrderBy(x => x.MixDescription) : model.OrderByDescending(x => x.MixDescription);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.MixID) : model.OrderByDescending(x => x.MixID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Mixes";
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