using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Ready Mix Manager,Dispatch Manager,Shop")]
    public class VehiclesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Vehicle";
            return View("Manage", new Vehicle
            {
                IsActive = true
            });
        }

        [HttpPost]
        public ActionResult Add(Vehicle model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Vehicles.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("TractorsSelectList");
                HttpContext.Cache.Remove("TrailersSelectList");
                HttpContext.Cache.Remove("VehiclesSelectList");
                TempData["Page.Title"] = "The Vehicle Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Vehicle Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Vehicle model = CarraraSQL.Vehicles.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Vehicle with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.VehicleLabel;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Vehicle model)
        {
            Vehicle entity = CarraraSQL.Vehicles.Find(model.VehicleID);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                try
                {
                    CarraraSQL.Vehicles.Attach(model);
                    CarraraSQL.Vehicles.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("TractorsSelectList");
                    HttpContext.Cache.Remove("TrailersSelectList");
                    HttpContext.Cache.Remove("VehiclesSelectList");
                    TempData["Page.Title"] = "The Vehicle Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Vehicle Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (model.Action == "Maintenance")
            {
                return RedirectToAction("Index", "VehicleMaintenance", new { vehicle = model.VehicleID });
            }
            if (ModelState.IsValid)
            {
                entity.IsActive = model.IsActive;
                entity.CurrentMeterReading = model.CurrentMeterReading;
                entity.DefaultDriverID = model.DefaultDriverID;
                entity.EZPass = model.EZPass;
                entity.GrossAxleWeightRating = model.GrossAxleWeightRating;
                entity.GrossVehicleWeightRating = model.GrossVehicleWeightRating;
                entity.IsOnRoad = model.IsOnRoad;
                entity.LastGrease = model.LastGrease;
                entity.LastOil = model.LastOil;
                entity.LastService = model.LastService;
                entity.Length = model.Length;
                entity.LocationID = model.LocationID;
                entity.Make = model.Make;
                entity.VehicleName = model.VehicleName;
                entity.NextGrease = model.NextGrease;
                entity.NextOil = model.NextOil;
                entity.NextService = model.NextService;
                entity.NumberOfAxles = model.NumberOfAxles;
                entity.Registration = model.Registration;
                entity.RegistrationDate = model.RegistrationDate;
                entity.RegistrationExpiration = model.RegistrationExpiration;
                entity.ShouldExportUsage = model.ShouldExportUsage;
                entity.TrailerType = model.TrailerType;
                entity.VehicleCode = model.VehicleCode;
                entity.VehicleTypeID = model.VehicleTypeID;
                entity.VIN = model.VIN;
                entity.Weight = model.Weight;
                entity.Year = model.Year;
                CarraraSQL.SaveChanges();
                Search.Index.VehiclesAsync();
                HttpContext.Cache.Remove("TractorsSelectList");
                HttpContext.Cache.Remove("TrailersSelectList");
                HttpContext.Cache.Remove("VehiclesSelectList");
                TempData["Page.Title"] = "The Vehicle Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Vehicle Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, int? location, string order, int? page, string query, int? show, string sort, int? trailer, int? type, int? vid)
        {
            IEnumerable<Vehicle> model = CarraraSQL.Vehicles;
            if (string.IsNullOrEmpty(query))
            {
                // Filters
                if (active.HasValue)
                {
                    model = model.Where(x => x.IsActive == active.Value);
                }
                if (location.HasValue)
                {
                    model = model.Where(x => x.LocationID == location.Value);
                }
                if (trailer.HasValue)
                {
                    model = model.Where(x => x.TrailerTypeID == trailer.Value);
                }
                if (type.HasValue)
                {
                    model = model.Where(x => x.VehicleTypeID == type.Value);
                }
                // Sort
                order = string.IsNullOrEmpty(order) ? "asc" : order;
                sort = string.IsNullOrEmpty(sort) ? "code" : sort;
                bool asc = order == "asc";
                switch (sort)
                {
                    case "name":
                        model = asc ? model.OrderBy(x => x.VehicleName) : model.OrderByDescending(x => x.VehicleName);
                        break;
                    case "account-number":
                        model = asc ? model.OrderBy(x => x.VehicleNumber) : model.OrderByDescending(x => x.VehicleNumber);
                        break;
                    case "vehicle-type":
                        model = asc ? model.OrderBy(x => x.VehicleType.VehicleTypeName) : model.OrderByDescending(x => x.VehicleType.VehicleTypeName);
                        break;
                    /*case "driver":
                        model = asc ?
                            model.OrderBy(x => !x.DefaultDriverID.HasValue).ThenBy(x => x.DefaultDriverID.HasValue ? x.Employee.LastFirst : "") :
                            model.OrderByDescending(x => x.DefaultDriverID.HasValue && x.Employee.LastFirst != null).ThenByDescending(x => x.DefaultDriverID.HasValue ? x.Employee.LastFirst : null);
                        break;*/
                    case "location":
                        model = asc ?
                            model.OrderBy(x => !x.LocationID.HasValue).ThenBy(x => x.LocationID.HasValue ? x.Location.LocationName : "") :
                            model.OrderByDescending(x => x.LocationID.HasValue && x.Location.LocationName != null).ThenByDescending(x => x.LocationID.HasValue ? x.Location.LocationName : null);
                        break;
                    case "ezpass":
                        model = asc ? model.OrderBy(x => x.EZPass) : model.OrderByDescending(x => x.EZPass);
                        break;
                    case "active":
                        model = asc ? model.OrderBy(x => x.IsActive) : model.OrderByDescending(x => x.IsActive);
                        break;
                    case "length":
                        model = asc ? model.OrderBy(x => x.Length) : model.OrderByDescending(x => x.Length);
                        break;
                    case "make":
                        model = asc ? model.OrderBy(x => x.Make) : model.OrderByDescending(x => x.Make);
                        break;
                    case "registration":
                        model = asc ? model.OrderBy(x => x.Registration) : model.OrderByDescending(x => x.Registration);
                        break;
                    case "registration-date":
                        model = asc ? model.OrderBy(x => x.RegistrationDate) : model.OrderByDescending(x => x.RegistrationDate);
                        break;
                    case "registration-expiration":
                        model = asc ? model.OrderBy(x => x.RegistrationExpiration) : model.OrderByDescending(x => x.RegistrationExpiration);
                        break;
                    case "vin":
                        model = asc ? model.OrderBy(x => x.VIN) : model.OrderByDescending(x => x.VIN);
                        break;
                    case "weight":
                        model = asc ? model.OrderBy(x => x.Weight) : model.OrderByDescending(x => x.Weight);
                        break;
                    case "year":
                        model = asc ? model.OrderBy(x => x.Year) : model.OrderByDescending(x => x.Year);
                        break;
                    case "trailer-type":
                        model = asc ?
                            model.OrderBy(x => !x.TrailerTypeID.HasValue).ThenBy(x => x.LocationID.HasValue ? x.TrailerType.TrailerTypeName : "") :
                            model.OrderByDescending(x => x.TrailerTypeID.HasValue && x.TrailerType.TrailerTypeName != null).ThenByDescending(x => x.TrailerTypeID.HasValue ? x.TrailerType.TrailerTypeName : null);
                        break;
                    case "next-oil":
                        model = asc ? model.OrderBy(x => x.NextOil) : model.OrderByDescending(x => x.NextOil);
                        break;
                    case "next-grease":
                        model = asc ? model.OrderBy(x => x.NextGrease) : model.OrderByDescending(x => x.NextGrease);
                        break;
                    case "next-service":
                        model = asc ? model.OrderBy(x => x.NextService) : model.OrderByDescending(x => x.NextService);
                        break;
                    case "current-meter-reading":
                        model = asc ? model.OrderBy(x => x.CurrentMeterReading) : model.OrderByDescending(x => x.CurrentMeterReading);
                        break;
                    case "last-oil":
                        model = asc ? model.OrderBy(x => x.LastOil) : model.OrderByDescending(x => x.LastOil);
                        break;
                    case "last-grease":
                        model = asc ? model.OrderBy(x => x.LastGrease) : model.OrderByDescending(x => x.LastGrease);
                        break;
                    case "last-service":
                        model = asc ? model.OrderBy(x => x.LastService) : model.OrderByDescending(x => x.LastService);
                        break;
                    case "should-export-usage":
                        model = asc ? model.OrderBy(x => x.ShouldExportUsage) : model.OrderByDescending(x => x.ShouldExportUsage);
                        break;
                    case "is-on-road":
                        model = asc ? model.OrderBy(x => x.IsOnRoad) : model.OrderByDescending(x => x.IsOnRoad);
                        break;
                    case "number-of-axles":
                        model = asc ? model.OrderBy(x => x.NumberOfAxles) : model.OrderByDescending(x => x.NumberOfAxles);
                        break;
                    case "gross-axle-weight-rating":
                        model = asc ? model.OrderBy(x => x.GrossAxleWeightRating) : model.OrderByDescending(x => x.GrossAxleWeightRating);
                        break;
                    case "gross-vehicle-weight-rating":
                        model = asc ? model.OrderBy(x => x.GrossVehicleWeightRating) : model.OrderByDescending(x => x.GrossVehicleWeightRating);
                        break;
                    default:
                        model = asc ? model.OrderBy(x => x.VehicleCode) : model.OrderByDescending(x => x.VehicleCode);
                        break;
                }
            }
            else
            {
                if (vid.HasValue)
                {
                    Vehicle vehicle = CarraraSQL.Vehicles.Find(vid);
                    if (vehicle != null)
                    {
                        RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
                        return RedirectToAction("Edit", parameters.CopyAndAdd("guid", vid));
                    }
                }
                // Search by query
                Search.Results<Vehicle> search = Search.Vehicles(active, CarraraSQL, query);
                model = search.List;
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : string.IsNullOrEmpty(search.DidYouMean) ? "Found " + search.TotalHits + " result" + (search.TotalHits == 1 ? "" : "s") + " for ‘" + query + "’" : "Did You Mean ‘" + search.DidYouMean + "’?";
                ViewBag.Results = search.TotalHits;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Vehicles";
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

            ViewBag.Columns = GetColumns<Vehicle>();
        }
        #endregion
    }
}