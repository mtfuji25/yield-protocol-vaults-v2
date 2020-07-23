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
    public class LoadTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Load Type";
            return View("Manage", new LoadType());
        }

        [HttpPost]
        public ActionResult Add(LoadType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.LoadTypes.Add(model);
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Load Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Load Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            LoadType model = CarraraSQL.LoadTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Load Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.LoadTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(LoadType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                LoadType entity = CarraraSQL.LoadTypes.Find(model.LoadTypeID);
                if (entity.Loads.Any())
                {
                    Page.Title = "The Load Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Load Type to Inactive if you do not intend to use this Load Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.LoadTypes.Attach(model);
                    CarraraSQL.LoadTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    TempData["Page.Title"] = "The Load Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Load Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Load Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Load Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<LoadType> model = CarraraSQL.LoadTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.LoadTypeName) : model.OrderByDescending(x => x.LoadTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.LoadTypeID) : model.OrderByDescending(x => x.LoadTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Load Types";
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