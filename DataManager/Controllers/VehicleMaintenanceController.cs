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
    [Authorize(Roles = "Admin,Ready Mix Manager,Dispatch Manager,Shop")]
    public class VehicleMaintenanceController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Vehicle Maintenance Record";
            return View("Manage", new VehicleMaintenance
            {
                DateOfService = DateTime.Now
            });
        }

        [HttpPost]
        public ActionResult Add(VehicleMaintenance model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.VehicleMaintenances.Add(model);
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Vehicle Maintenance Record Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Vehicle Maintenance Record Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            VehicleMaintenance model = CarraraSQL.VehicleMaintenances.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Vehicle Maintenance Record with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.DateOfService.ToShortDateString() + " Maintenance Record for " + model.Vehicle.VehicleLabel;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(VehicleMaintenance model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                try
                {
                    CarraraSQL.VehicleMaintenances.Attach(model);
                    CarraraSQL.VehicleMaintenances.Remove(model);
                    CarraraSQL.SaveChanges();
                    TempData["Page.Title"] = "The Vehicle Maintenance Record Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Vehicle Maintenance Record Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Vehicle Maintenance Record Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Vehicle Maintenance Record Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort, int? vehicle)
        {
            IEnumerable<VehicleMaintenance> model = CarraraSQL.VehicleMaintenances;
            // Filter by active/inactive
            if (vehicle.HasValue)
            {
                model = model.Where(x => x.VehicleID == vehicle.Value);
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "date-of-service" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "vehicle":
                    model = asc ? model.OrderBy(x => x.Vehicle.VehicleCode) : model.OrderByDescending(x => x.Vehicle.VehicleCode);
                    break;
                case "meter":
                    model = asc ? model.OrderBy(x => x.Meter) : model.OrderByDescending(x => x.Meter);
                    break;
                case "fuel-amount":
                    model = asc ? model.OrderBy(x => x.FuelAmount) : model.OrderByDescending(x => x.FuelAmount);
                    break;

                case "mpg":
                    // TODO
                    break;
                case "oil":
                    model = asc ? model.OrderBy(x => x.Oil.HasValue && x.Oil.Value) : model.OrderByDescending(x => x.Oil.HasValue && x.Oil.Value);
                    break;
                case "grease":
                    model = asc ? model.OrderBy(x => x.Grease) : model.OrderByDescending(x => x.Grease);
                    break;
                case "service":
                    model = asc ? model.OrderBy(x => x.Service) : model.OrderByDescending(x => x.Service);
                    break;
                default:
                    model = asc ? model.OrderByDescending(x => x.DateOfService) : model.OrderBy(x => x.DateOfService);
                    break;
            }
            // Return Results
            if (vehicle.HasValue)
            {
                Vehicle entity = CarraraSQL.Vehicles.Find(vehicle.Value);
                Page.Title = "Vehicle Maintenance Records for " + entity.VehicleLabel;
            }
            else
            {
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Vehicle Maintenance Records";

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
        #endregion
    }
}