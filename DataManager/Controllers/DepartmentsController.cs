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
    public class DepartmentsController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Department";
            return View("Manage", new Department());
        }

        [HttpPost]
        public ActionResult Add(Department model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Departments.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("DepartmentsSelectList");
                TempData["Page.Title"] = "The Department Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Department Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Department model = CarraraSQL.Departments.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Department with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.DepartmentName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Department model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Department entity = CarraraSQL.Departments.Find(model.DepartmentID);
                if (entity.Employees.Any() ||
                    entity.HoursAssignments.Any() ||
                    entity.Loads.Any() ||
                    entity.TimeClockPermissions.Any())
                {
                    Page.Title = "The Department Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Department to Inactive if you do not intend to use this Department again.";
                    return View("Manage", model);
                }

                try
                {
                    CarraraSQL.Departments.Attach(model);
                    CarraraSQL.Departments.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("DepartmentsSelectList");
                    TempData["Page.Title"] = "The Department Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Department Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("DepartmentsSelectList");
                TempData["Page.Title"] = "The Department Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Department Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<Department> model = CarraraSQL.Departments;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "code":
                    model = asc ? model.OrderBy(x => x.DepartmentCode) : model.OrderByDescending(x => x.DepartmentCode);
                    break;
                case "name":
                    model = asc ? model.OrderBy(x => x.DepartmentName) : model.OrderByDescending(x => x.DepartmentName);
                    break;
                case "viewpoint-offset-account":
                     model = asc ? model.OrderBy(x => x.ViewPointOffsetAccount) : model.OrderByDescending(x => x.ViewPointOffsetAccount);
                    break;
                case "default-cost-center":
                    model = asc ? model.OrderBy(x => x.DefaultCostCenterID) : model.OrderByDescending(x => x.DefaultCostCenterID);
                    break;
                case "default-lunch":
                    model = asc ? model.OrderBy(x => x.DefaultLunch) : model.OrderByDescending(x => x.DefaultLunch);
                    break;
                case "auto-adjust-clock-in":
                    model = asc ? model.OrderBy(x => x.AutoAdjustClockIn) : model.OrderByDescending(x => x.AutoAdjustClockIn);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.DepartmentID) : model.OrderByDescending(x => x.DepartmentID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Departments";
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