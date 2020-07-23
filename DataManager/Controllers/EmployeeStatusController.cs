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
    public class EmployeeStatusController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Employee Status";
            return View("Manage", new EmployeeStatus());
        }

        [HttpPost]
        public ActionResult Add(EmployeeStatus model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.EmployeeStatuses.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("EmployeeStatusSelectList");
                TempData["Page.Title"] = "The Employee Status Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Employee Status Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            EmployeeStatus model = CarraraSQL.EmployeeStatuses.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Employee Status with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.EmployeeStatusName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeStatus model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                EmployeeStatus entity = CarraraSQL.EmployeeStatuses.Find(model.EmployeeStatusID);
                if (entity.Employees.Any())
                {
                    Page.Title = "The Employee Status Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Employee Status to Inactive if you do not intend to use this Employee Status again.";
                    return View("Manage", model);
                }

                try
                {
                    CarraraSQL.EmployeeStatuses.Attach(model);
                    CarraraSQL.EmployeeStatuses.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("EmployeeStatusSelectList");
                    TempData["Page.Title"] = "The Employee Status Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Employee Status Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("EmployeeStatusSelectList");
                TempData["Page.Title"] = "The Employee Status Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Employee Status Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<EmployeeStatus> model = CarraraSQL.EmployeeStatuses;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.EmployeeStatusName) : model.OrderByDescending(x => x.EmployeeStatusName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.EmployeeStatusID) : model.OrderByDescending(x => x.EmployeeStatusID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Employee Status";
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