using DataManager.Models;
using DataManager.Models.CarraraSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DataManager.Controllers
{
    public class TimeClockController : DataManagerController
    {
        #region Methods
        public TimeClockController()
        {
            Page.Id = "timeclock";
        }

        public ActionResult Index()
        {
            List<string> allowedIps = DataContext.TouchScreens.Where(x => x.Active).Select(x => x.IPAddress).ToList();
            if (HttpContext.IsDebuggingEnabled)
            {
                allowedIps.Add("::1");
            }
            if (!allowedIps.Contains(GetIP()))
            {
                return Redirect("http://jpcarrara.com/");
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : Site.Name + " Time Clock";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Please enter your employee number:";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string EmployeeNumber)
        {
            Employee model = CarraraSQL.Employees.FirstOrDefault(x => x.EmployeeNumber == EmployeeNumber && x.EmployeeStatusID == 1);
            if (model == null)
            {
                Page.Title = Site.Name + " Time Clock";
                Page.Message = "Please enter your employee number:";
                ModelState.AddModelError("", "The employee number you entered was not found in our system. Please try again.");
                return View(model);
            }
            Page.Title = Site.Name + " Time Clock";
            Page.Message = "Welcome " + model.FullName + ", please clock in or out:";
            return View("Clock", model);
        }

        [HttpPost]
        public ActionResult Clock(string Clock, int EmployeeID)
        {
            if (Clock == "Clock In")
            {
                CarraraSQL.Database.SqlQuery<object>("ClockIn", EmployeeID, DateTime.Now);
            }
            else
            {
                CarraraSQL.Database.SqlQuery<object>("ClockOut", EmployeeID, DateTime.Now);
            }
            Employee employee = CarraraSQL.Employees.Find(EmployeeID);
            TempData["Page.Title"] = employee.FullName + " has been successfully clocked " + (Clock == "Clock In" ? "in" : "out") + ".";
            return RedirectToAction("Index");
        }
        #endregion
    }
}