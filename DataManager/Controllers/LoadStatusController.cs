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
    public class LoadStatusController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Load Status";
            return View("Manage", new LoadStatu());
        }

        [HttpPost]
        public ActionResult Add(LoadStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.LoadStatus.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("LoadStatusSelectList");
                TempData["Page.Title"] = "The Load Status Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Load Status Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            LoadStatu model = CarraraSQL.LoadStatus.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Load Status with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.LoadStatusName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(LoadStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                LoadStatu entity = CarraraSQL.LoadStatus.Find(model.LoadStatusID);
                if (entity.Loads.Any())
                {
                    Page.Title = "The Load Status Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Load Status to Inactive if you do not intend to use this Load Status again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.LoadStatus.Attach(model);
                    CarraraSQL.LoadStatus.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("LoadStatusSelectList");
                    TempData["Page.Title"] = "The Load Status Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Load Status Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("LoadStatusSelectList");
                TempData["Page.Title"] = "The Load Status Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Load Status Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<LoadStatu> model = CarraraSQL.LoadStatus;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.LoadStatusName) : model.OrderByDescending(x => x.LoadStatusName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.LoadStatusID) : model.OrderByDescending(x => x.LoadStatusID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Load Status";
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