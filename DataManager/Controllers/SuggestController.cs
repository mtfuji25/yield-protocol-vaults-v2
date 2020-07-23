using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DataManager.Controllers
{
    public class SuggestController : DataManagerController
    {
        #region Methods
        public ContentResult Contacts(string term, int? max)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Content("{}", "application/json");
            }
            term = term.Trim().ToLower();
            string first = term;
            string last = term;
            if (term.Contains(" "))
            {
                char[] delimiters = { ' ' };
                first = term.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).First();
                last = term.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            var suggestions = (from x in CarraraSQL.Contacts
                               where x.ContactTypeID != 14 &&
                               (x.FirstName.ToLower().Contains(first) || x.LastName.ToLower().Contains(last) || (!string.IsNullOrEmpty(x.Company) && x.Company.ToLower().Contains(term)))
                               select new
                               {
                                   name = x.DisplayName,
                                   label = x.DisplayName,
                                   value = x.ContactID
                               }).Take(max ?? 100).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Content(serializer.Serialize(suggestions.OrderBy(x => QueryOrder(term, x.name)).ToList()), "application/json");
        }

        private int QueryOrder(string query, string name)
        {
            name = Regex.Replace(name.ToLower(), @"\[[^]]*\]", string.Empty).Trim();
            if (name == query)
            {
                return -1;
            }
            if (name.Contains(query))
            {
                return 0;
            }
            return 1;
        }

        public ContentResult Employees(bool? driver, bool? manager, int? max, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Content("{}", "application/json");
            }
            term = term.Trim().ToLower();
            string first = term;
            string last = term;
            if (term.Contains(" "))
            {
                char[] delimiters = { ' ' };
                first = term.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).First();
                last = term.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            var suggestions = driver.HasValue && driver.Value ?
                           (from x in CarraraSQL.Employees.AsNoTracking()
                            let label = x.LastName + ", " + x.FirstName
                            where x.EmployeeStatusID == 1 && x.IsDriver &&
                            (x.FirstName.ToLower().Contains(first) || x.LastName.ToLower().Contains(last) || x.EmployeeNumber.Contains(first) || x.EmployeeNumber.Contains(first))
                            select new
                            {
                                name = label,
                                label = label,
                                value = x.EmployeeID
                            }).Take(max ?? 100).ToList()
                            : manager.HasValue && manager.Value ?
                           (from x in CarraraSQL.Employees.AsNoTracking()
                            let label = x.LastName + ", " + x.FirstName
                            where x.EmployeeStatusID == 1 && x.IsProjectManager &&
                            (x.FirstName.ToLower().Contains(first) || x.LastName.ToLower().Contains(last) || x.EmployeeNumber.Contains(first) || x.EmployeeNumber.Contains(first))
                            select new
                            {
                                name = label,
                                label = label,
                                value = x.EmployeeID
                            }).Take(max ?? 100).ToList()
                            :
                            (from x in CarraraSQL.Employees.AsNoTracking()
                             let label = x.LastName + ", " + x.FirstName
                             where x.EmployeeStatusID == 1 && (x.FirstName.ToLower().Contains(first) || x.LastName.ToLower().Contains(last) || x.EmployeeNumber.Contains(first))
                             select new
                             {
                                 name = label,
                                 label = label,
                                 value = x.EmployeeID
                             }).Take(max ?? 100).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Content(serializer.Serialize(suggestions.OrderBy(x => QueryOrder(term, x.name)).ToList()), "application/json");
        }

        public ContentResult Jobs(string term, int? max)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Content("{}", "application/json");
            }
            term = term.Trim().ToLower();
            var suggestions = (from x in CarraraSQL.Jobs
                               let name = x.JobNumber + " " + x.JobName
                               where x.JobStatusID.HasValue && x.JobStatusID.Value == 1 &&
                               ((!string.IsNullOrEmpty(x.JobName) && x.JobName.ToLower().Contains(term)) || (!string.IsNullOrEmpty(x.JobNumber) && x.JobNumber.ToLower().Contains(term)))
                               select new
                               {
                                   name = name,
                                   label = name,
                                   value = x.JobID
                               }).Take(max ?? 100).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Content(serializer.Serialize(suggestions.OrderBy(x => QueryOrder(term, x.name)).ToList()), "application/json");
        }

        public ContentResult Vehicles(string term, int? max)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Content("{}", "application/json");
            }
            term = term.Trim().ToLower();
            var suggestions = (from x in CarraraSQL.Vehicles
                               where x.IsActive &&
                               ((!string.IsNullOrEmpty(x.VehicleCode) && x.VehicleCode.ToLower().Contains(term)) || (!string.IsNullOrEmpty(x.VehicleName) && x.VehicleName.ToLower().Contains(term)))
                               select new
                               {
                                   name = x.VehicleCode,
                                   label = x.VehicleCode,
                                   value = x.VehicleID
                               }).Take(max ?? 100).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Content(serializer.Serialize(suggestions.OrderBy(x => QueryOrder(term, x.name)).ToList()), "application/json");
        }
        #endregion
    }
}