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
    public class MeterTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Meter Type";
            return View("Manage", new MeterType());
        }

        [HttpPost]
        public ActionResult Add(MeterType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.MeterTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MeterTypesSelectList");
                TempData["Page.Title"] = "The Meter Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Meter Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            MeterType model = CarraraSQL.MeterTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Meter Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.MeterTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(MeterType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                MeterType entity = CarraraSQL.MeterTypes.Find(model.MeterTypeID);
                if (entity.VehicleTypes.Any())
                {
                    Page.Title = "The Meter Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Meter Type to Inactive if you do not intend to use this Meter Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.MeterTypes.Attach(model);
                    CarraraSQL.MeterTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("MeterTypesSelectList");
                    TempData["Page.Title"] = "The Meter Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Meter Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MeterTypesSelectList");
                TempData["Page.Title"] = "The Meter Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Meter Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<MeterType> model = CarraraSQL.MeterTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.MeterTypeName) : model.OrderByDescending(x => x.MeterTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.MeterTypeID) : model.OrderByDescending(x => x.MeterTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Meter Types";
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