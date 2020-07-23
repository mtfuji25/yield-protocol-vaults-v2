using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TouchScreensController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Touch Screen";
            return View("Manage", new TouchScreen());
        }

        [HttpPost]
        public ActionResult Add(TouchScreen model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                DataContext.TouchScreens.Add(model);
                DataContext.SaveChanges();
                TempData["Page.Title"] = "The Touch Screen Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Touch Screen Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            TouchScreen model = DataContext.TouchScreens.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Touch Screen with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.TouchScreenName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(TouchScreen model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            TouchScreen entity = DataContext.TouchScreens.Find(model.TouchScreenID);
            if (model.Action == "Delete")
            {
                try
                {
                    DataContext.TouchScreens.Remove(entity);
                    DataContext.SaveChanges();
                    TempData["Page.Title"] = "The Touch Screen Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Touch Screen Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                entity.Active = model.Active;
                entity.IPAddress = model.IPAddress;
                entity.TouchScreenName = model.TouchScreenName;
                DataContext.SaveChanges();
                TempData["Page.Title"] = "The Touch Screen Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Touch Screen Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, string order, int? page, bool? refresh, int? show, string sort)
        {
            IEnumerable<TouchScreen> model = DataContext.TouchScreens;
            // Filter by active/inactive
            if (active.HasValue)
            {
                model = model.Where(x => x.Active == active.Value);
            }
            if (refresh.HasValue && refresh.Value)
            {
                foreach (TouchScreen item in model.ToList())
                {
                    Ping pinger = new Ping();
                    try
                    {
                        PingReply reply = pinger.Send(item.IPAddress);
                        item.HttpStatus = reply.Status.ToString(); // == IPStatus.Success;
                        item.PingException = string.Empty;
                        item.LastStatusCheck = DateTime.Now;
                    }
                    catch (PingException ex)
                    {
                        item.PingException = ex.Message + " " + ex.StackTrace;
                    }
                }
                DataContext.SaveChanges();
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "active":
                    model = asc ? model.OrderBy(x => x.Active) : model.OrderByDescending(x => x.Active);
                    break;
                case "name":
                    model = asc ? model.OrderBy(x => x.TouchScreenName) : model.OrderByDescending(x => x.TouchScreenName);
                    break;
                case "last-sync":
                    model = asc ? model.OrderByDescending(x => x.LastSync) : model.OrderBy(x => x.LastSync);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.TouchScreenID) : model.OrderByDescending(x => x.TouchScreenID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Touch Screens";
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

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
        }
        #endregion
    }
}