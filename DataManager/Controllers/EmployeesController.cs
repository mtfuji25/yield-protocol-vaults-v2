using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Precast Manager,Ready-Mix Manager")]
    public class EmployeesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add A New Employee";
            return View("Manage", new Employee());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(Employee model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            Employee existing = CarraraSQL.Employees.FirstOrDefault(x => x.EmployeeNumber == model.EmployeeNumber);
            if (existing != null)
            {
                ModelState.AddModelError("", "This employee number is already in use by an active employee. Please use a unique employee number.");
            }
            if (TryValidateModel(model))
            {
                CarraraSQL.Employees.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("DriversSelectList");
                HttpContext.Cache.Remove("EmployeesSelectList");
                TempData["Page.Title"] = "The Employee Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Employee Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Employee model = CarraraSQL.Employees.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Employee with Id " + guid + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.FullName;
            return View("Manage", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Employee model)
        {
            Employee entity = CarraraSQL.Employees.Find(model.EmployeeID);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            Employee existing = CarraraSQL.Employees.FirstOrDefault(x => x.EmployeeNumber == model.EmployeeNumber && x.EmployeeID != model.EmployeeID);
            if (existing != null)
            {
                ModelState.AddModelError("", "That employee number is already in use by an active employee. Please use a unique employee number.");
            }
            if (TryValidateModel(model))
            {
                entity.BirthDate = model.BirthDate;
                entity.City = model.City;
                entity.DefaultAssignmentTypeID = model.DefaultAssignmentTypeID;
                entity.DefaultCostCenterID = model.DefaultCostCenterID;
                entity.DepartmentID= model.DepartmentID;
                entity.DriverKey = model.DriverKey;
                entity.EmergencyContactName = model.EmergencyContactName;
                entity.EmergencyContactPhone = model.EmergencyContactPhone;
                entity.EmployeeNumber = model.EmployeeNumber;
                entity.EmployeeStatusID = model.EmployeeStatusID;
                entity.EndDate = model.EndDate;
                entity.FirstName = model.FirstName;
                entity.IsAdministrator = model.IsAdministrator;
                entity.IsDriver = model.IsDriver;
                entity.IsHourly = model.IsHourly;
                entity.IsProjectManager = model.IsProjectManager;
                entity.IsRediMixVisible = model.IsRediMixVisible;
                entity.LastName = model.LastName;
                entity.LocationID = model.LocationID;
                entity.Mobile = model.Mobile;
                entity.Phone = model.Phone;
                entity.Rate = model.Rate;
                entity.ShiftEndTime = model.ShiftEndTime;
                entity.ShiftStartTime = model.ShiftStartTime;
                entity.StartDate = model.StartDate;
                entity.Street = model.Street;
                entity.SSN = model.SSN;
                entity.State = model.State;
                entity.VacationHours = model.VacationHours;
                entity.VehicleID = model.VehicleID;
                entity.Zip = model.Zip;
                CarraraSQL.SaveChanges();
                Search.Index.EmployeesAsync();
                HttpContext.Cache.Remove("DriversSelectList");
                HttpContext.Cache.Remove("EmployeesSelectList");
                TempData["Page.Title"] = "The Employee Was Updated Successfully!";
                return RedirectToAction("Edit", parameters.CopyAndAdd("guid", model.EmployeeID));
            }
            Page.Title = "The Employee Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, int? department, int? location, string order, int? page, string query, int? show, string sort, int? status, string uid)
        {
            IEnumerable<Employee> model = CarraraSQL.Employees;
            if (string.IsNullOrEmpty(query))
            {
                // Filter
                if (department.HasValue)
                {
                    model = model.Where(x => x.DepartmentID == department.Value);
                }
                if (location.HasValue)
                {
                    model = model.Where(x => x.LocationID == location.Value);
                }
                if (status.HasValue)
                {
                    model = model.Where(x => x.EmployeeStatusID == status.Value);
                }
                else
                {
                    model = model.Where(x => x.EmployeeStatusID.Value != 2);
                }
                // Sort
                order = string.IsNullOrEmpty(order) ? "asc" : order;
                sort = string.IsNullOrEmpty(sort) ? "employee-number" : sort;
                bool asc = order == "asc";
                switch (sort)
                {
                    case "last":
                        model = asc ? model.OrderBy(x => x.LastName).ThenBy(x => x.FirstName) : model.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName);
                        break;
                    case "first":
                        model = asc ? model.OrderBy(x => x.FirstName).ThenBy(x => x.LastName) : model.OrderByDescending(x => x.FirstName).ThenBy(x => x.LastName);
                        break;
                    case "mobile":
                        model = asc ? model.OrderBy(x => x.Mobile) : model.OrderByDescending(x => x.Mobile);
                        break;
                    case "phone":
                        model = asc ? model.OrderBy(x => x.Phone) : model.OrderByDescending(x => x.Phone);
                        break;

                    case "street":
                        model = asc ? model.OrderBy(x => x.Street) : model.OrderByDescending(x => x.Street);
                        break;
                    case "city":
                        model = asc ? model.OrderBy(x => x.City) : model.OrderByDescending(x => x.City);
                        break;
                    case "state":
                        model = asc ? model.OrderBy(x => x.State) : model.OrderByDescending(x => x.State);
                        break;
                    case "zip":
                        model = asc ? model.OrderBy(x => x.Zip) : model.OrderByDescending(x => x.Zip);
                        break;
                    case "birth-date":
                        model = asc ? model.OrderByDescending(x => x.BirthDate) : model.OrderBy(x => x.BirthDate);
                        break;
                    case "department":
                        model = asc ?
                            model.OrderBy(x => !x.DepartmentID.HasValue).ThenBy(x => x.DepartmentID.HasValue ? x.Department.DepartmentName : "") :
                            model.OrderByDescending(x => x.DepartmentID.HasValue && x.Department.DepartmentName != null).ThenByDescending(x => x.DepartmentID.HasValue ? x.Department.DepartmentName : null);
                        break;
                    case "location":
                        model = asc ?
                            model.OrderBy(x => !x.LocationID.HasValue).ThenBy(x => x.LocationID.HasValue ? x.Location.LocationName : "") :
                            model.OrderByDescending(x => x.LocationID.HasValue && x.Location.LocationName != null).ThenByDescending(x => x.LocationID.HasValue ? x.Location.LocationName : null);
                        break;
                    case "default-cost-center":
                        model = asc ? model.OrderBy(x => x.DefaultCostCenterID) : model.OrderByDescending(x => x.DefaultCostCenterID);
                        break;
                    case "shift-start-time":
                        model = asc ? model.OrderBy(x => x.ShiftStartTime) : model.OrderByDescending(x => x.ShiftStartTime);
                        break;
                    case "shift-end-time":
                        model = asc ? model.OrderBy(x => x.ShiftEndTime) : model.OrderByDescending(x => x.ShiftEndTime);
                        break;
                    case "hourly":
                        model = asc ? model.OrderBy(x => x.IsHourly) : model.OrderByDescending(x => x.IsHourly);
                        break;
                    case "project-manager":
                        model = asc ? model.OrderBy(x => x.IsProjectManager) : model.OrderByDescending(x => x.IsProjectManager);
                        break;
                    case "driver":
                        model = asc ? model.OrderBy(x => x.IsDriver) : model.OrderByDescending(x => x.IsDriver);
                        break;
                    case "employee-status":
                        model = asc ?
                            model.OrderBy(x => !x.EmployeeStatusID.HasValue).ThenBy(x => x.EmployeeStatusID.HasValue ? x.EmployeeStatus.EmployeeStatusName : "") :
                            model.OrderByDescending(x => x.EmployeeStatusID.HasValue && x.EmployeeStatus.EmployeeStatusName != null).ThenByDescending(x => x.EmployeeStatusID.HasValue ? x.EmployeeStatus.EmployeeStatusName : null);
                        break;

                    case "start-date":
                        model = asc ? model.OrderByDescending(x => x.StartDate) : model.OrderBy(x => x.StartDate);
                        break;
                    case "end-date":
                        model = asc ? model.OrderByDescending(x => x.EndDate) : model.OrderBy(x => x.EndDate);
                        break;
                    case "vacation-hours":
                        model = asc ? model.OrderBy(x => x.VacationHours) : model.OrderByDescending(x => x.VacationHours);
                        break;

                    case "years-of-service":
                        model = asc ? model.OrderBy(x => x.YearsOfService) : model.OrderByDescending(x => x.YearsOfService);
                        break;
                    case "vehicle":
                        model = asc ?
                            model.OrderBy(x => !x.VehicleID.HasValue).ThenBy(x => x.VehicleID.HasValue ? x.Vehicle.VehicleLabel : "") :
                            model.OrderByDescending(x => x.VehicleID.HasValue && x.Vehicle.VehicleLabel != null).ThenByDescending(x => x.VehicleID.HasValue ? x.Vehicle.VehicleLabel : null);
                        break;
                    case "rate":
                        model = asc ? model.OrderBy(x => x.Rate) : model.OrderByDescending(x => x.Rate);
                        break;
                    case "emergency-contact-name":
                        model = asc ? model.OrderBy(x => x.EmergencyContactName) : model.OrderByDescending(x => x.EmergencyContactName);
                        break;
                    case "emergency-contact-phone":
                        model = asc ? model.OrderBy(x => x.EmergencyContactPhone) : model.OrderByDescending(x => x.EmergencyContactPhone);
                        break;

                    default:
                        model = asc ? model.OrderBy(x => x.EmployeeNumber) : model.OrderByDescending(x => x.EmployeeNumber);
                        break;

                }
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Employees";
            }
            else
            {
                if (!string.IsNullOrEmpty(uid))
                {
                    Employee employee = CarraraSQL.Employees.Find(uid);
                    if (employee != null)
                    {
                        RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
                        return RedirectToAction("Edit", parameters.CopyAndAdd("guid", uid));
                    }
                }
                // Search by query
                Search.Results<Employee> search = Search.Employees(CarraraSQL, query);
                model = search.List;
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : string.IsNullOrEmpty(search.DidYouMean) ? "Found " + search.TotalHits + " result" + (search.TotalHits == 1 ? "" : "s") + " for ‘" + query + "’" : "Did You Mean ‘" + search.DidYouMean + "’?";
                ViewBag.Results = search.TotalHits;
            }
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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.Columns = GetColumns<Employee>();
        }
        #endregion
    }
}