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
    public class MarkSizeTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Mark Size Type";
            return View("Manage", new MarkSizeType());
        }

        [HttpPost]
        public ActionResult Add(MarkSizeType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.MarkSizeTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MarkSizeTypesSelectList");
                TempData["Page.Title"] = "The Mark Size Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Mark Size Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            MarkSizeType model = CarraraSQL.MarkSizeTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Mark Size Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.MarkSizeTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(MarkSizeType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                MarkSizeType entity = CarraraSQL.MarkSizeTypes.Find(model.MarkSizeTypeID);
                if (entity.MarkTypes.Any())
                {
                    Page.Title = "The Mark Size Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Mark Size Type to Inactive if you do not intend to use this Mark Size Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.MarkSizeTypes.Attach(model);
                    CarraraSQL.MarkSizeTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("MarkSizeTypesSelectList");
                    TempData["Page.Title"] = "The Mark Size Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Mark Size Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MarkSizeTypesSelectList");
                TempData["Page.Title"] = "The Mark Size Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Mark Size Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<MarkSizeType> model = CarraraSQL.MarkSizeTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.MarkSizeTypeName) : model.OrderByDescending(x => x.MarkSizeTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.MarkSizeTypeID) : model.OrderByDescending(x => x.MarkSizeTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Mark Size Types";
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