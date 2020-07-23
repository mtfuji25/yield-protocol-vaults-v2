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
    public class PourStatusController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Pour Status";
            return View("Manage", new PourStatu());
        }

        [HttpPost]
        public ActionResult Add(PourStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.PourStatus.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("PourStatusSelectList");
                TempData["Page.Title"] = "The Pour Status Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Pour Status Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            PourStatu model = CarraraSQL.PourStatus.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Pour Status with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.PourStatusName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(PourStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                PourStatu entity = CarraraSQL.PourStatus.Find(model.PourStatusID);
                if (entity.Pours.Any())
                {
                    Page.Title = "The Pour Status Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Pour Status to Inactive if you do not intend to use this Pour Status again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.PourStatus.Attach(model);
                    CarraraSQL.PourStatus.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("PourStatusSelectList");
                    TempData["Page.Title"] = "The Pour Status Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Pour Status Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("PourStatusSelectList");
                TempData["Page.Title"] = "The Pour Status Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Pour Status Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<PourStatu> model = CarraraSQL.PourStatus;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.PourStatusName) : model.OrderByDescending(x => x.PourStatusName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.PourStatusID) : model.OrderByDescending(x => x.PourStatusID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Pour Status";
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