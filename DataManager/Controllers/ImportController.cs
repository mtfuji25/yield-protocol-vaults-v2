using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DataManager.Controllers
{
    public class ImportController : DataManagerController
    {
        // GET: Import

        public string GetEmployeeId(int id)
        {
            switch (id)
            {
                case 298:
                    id = 434;
                    break;
                case 479:
                    id = 485;
                    break;
                case 248:
                    id = 490;
                    break;
                case 293:
                    id = 512;
                    break;
                case 277:
                    id = 513;
                    break;
                case 395:
                    id = 540;
                    break;
                case 328:
                    id = 564;
                    break;
                case 563:
                    id = 644;
                    break;
            }
            UserProfile user = DataContext.Users.FirstOrDefault(x => x.EmployeeId == id);
            if (user != null)
            {
                return user.Id;
            }
            return string.Empty;
        }

        public ActionResult Contacts()
        {
            int found = 0;
            int imported = 0;
            List<string> errors = new List<string>
            {
                Environment.NewLine
            };
            using (OldDataManager.Models.OldDataManager context = new OldDataManager.Models.OldDataManager())
            {
                var items = context.Contacts.ToList();
                found = items.Count();
                foreach (var item in items)
                {
                    Contact existing = DataContext.Contacts.FirstOrDefault(x => x.OriginalId == item.ContactID);
                    if (existing == null)
                    {
                        int? contactTypeId = null;
                        if (item.ContactTypeID.HasValue)
                        {
                            ContactType type = DataContext.ContactTypes.FirstOrDefault(x => x.OriginalId.Value == item.ContactTypeID.Value);
                            if (type != null)
                            {
                                contactTypeId = type.ContactTypeId;
                            }
                        }
                        try
                        {
                            DataContext.Contacts.Add(new Contact
                            {
                                Active = true,
                                Address1 = item.StreetAddress,
                                City = item.City,
                                Company = item.Company,
                                ContactTypeId = contactTypeId,
                                DisplayName = item.DisplayName,
                                Email = item.EMail,
                                FaxNumber = item.FaxNumber,
                                FirstName = item.FirstName,
                                HomePhone = item.HomePhone,
                                LastName = item.LastName,
                                LastUpdated = DateTime.Now,
                                MobilePhone = item.MobilePhone,
                                Notes = item.Notes,
                                OriginalId = item.ContactID,
                                State = item.State,
                                WorkPhone = item.WorkPhone,
                                ZipCode = item.Zip
                            });
                            DataContext.SaveChanges();
                            imported++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add(item.ContactID + " failed: " + ex.Message);
                        }
                    }
                }
            }
            return Content("Found " + found + ", imported " + imported + " Contacts.", "text/plain");
        }

        public ActionResult Departments()
        {
            int found = 0;
            int imported = 0;
            using (OldDataManager.Models.OldDataManager context = new OldDataManager.Models.OldDataManager())
            {
                var items = context.Departments.ToList();
                found = items.Count();
                foreach (var item in items)
                {
                    Department existing = DataContext.Departments.FirstOrDefault(x => x.OriginalId == item.DepartmentID);
                    if (existing == null)
                    {
                        DataContext.Departments.Add(new Department
                        {
                            Active = true,
                            AutoAdjustClockIn = item.AutoAdjustClockIn,
                            Code = item.DepartmentCode,
                            DefaultLunch = item.DefaultLunch,
                            DefaultAssignmentTypeId = item.DefaultAssignmentTypeID,
                            DefaultCostCenterId = item.DefaultCostCenterID,
                            Name = item.DepartmentName,
                            OriginalId = item.DepartmentID,
                            ViewPointOffsetAccount = item.ViewPointOffsetAccount
                        });
                        DataContext.SaveChanges();
                        imported++;
                    }
                }
            }
            return Content("Found " + found + ", imported " + imported + " Departments.", "text/plain");
        }

        public async Task<ActionResult> Employees()
        {
            //TODO: WILL NEED TO UPDATE DEFAULTDRIVERID ON VEHICLES AFTER IMPORT
            int found = 0;
            int imported = 0;
            StringBuilder errors = new StringBuilder();
            using (OldDataManager.Models.OldDataManager context = new OldDataManager.Models.OldDataManager())
            {
                var items = context.Employees.ToList();
                found = items.Count();
                foreach (var item in items)
                {
                    if (item.EmployeeID == 479 || 
                        item.EmployeeID == 298 || 
                        item.EmployeeID == 248 || 
                        item.EmployeeID == 512 || 
                        item.EmployeeID == 277 || 
                        item.EmployeeID == 395 || 
                        item.EmployeeID == 328 ||
                        item.EmployeeID == 563)
                    {
                        continue;
                    }
                    UserProfile existing = DataContext.Users.FirstOrDefault(x => x.EmployeeId == item.EmployeeID);
                    if (existing == null)
                    {
                        int? departmentId = null;
                        if (item.DepartmentID.HasValue)
                        {
                            Department department = DataContext.Departments.FirstOrDefault(x => x.OriginalId == item.DepartmentID);
                            if (department != null)
                            {
                                departmentId = department.DepartmentId;
                            }
                        }

                        int? employeeStatusId = null;
                        if (item.EmployeeStatusID.HasValue)
                        {
                            EmployeeStatus status = DataContext.EmployeeStatuses.FirstOrDefault(x => x.OriginalId == item.EmployeeStatusID);
                            if (status != null)
                            {
                                employeeStatusId = status.EmployeeStatusId;
                            }
                        }

                        int? locationId = null;
                        if (item.LocationID.HasValue)
                        {
                            Location type = DataContext.Locations.FirstOrDefault(x => x.OriginalId.Value == item.LocationID);
                            if (type != null)
                            {
                                locationId = type.LocationId;
                            }
                        }

                        int? vehicleId = null;
                        if (item.LocationID.HasValue)
                        {
                            Vehicle vehicle = DataContext.Vehicles.FirstOrDefault(x => x.OriginalId.Value == item.VehicleID);
                            if (vehicle != null)
                            {
                                vehicleId = vehicle.VehicleId;
                            }
                        }
                        int? employeeNumber = null;
                        if (!string.IsNullOrEmpty(item.EmployeeNumber))
                        {
                            try
                            {
                                employeeNumber = Convert.ToInt32(item.EmployeeNumber);
                            }
                            catch { }
                        }

                        string email = item.FirstName.ToLower().ToSafeUrl() + "." + item.LastName.ToLower().ToSafeUrl() + "@jpcarrara.com";
                        try
                        {
                            UserProfile model = new UserProfile
                            {
                                Active = employeeStatusId.HasValue ? employeeStatusId == 1 : false,
                                Address1 = item.Street,
                                BirthDate = item.BirthDate,
                                City = item.City,
                                DefaultAssignmentTypeId = item.DefaultAssignmentTypeID,
                                DefaultCostCenterId = item.DefaultCostCenterID,
                                DepartmentId = departmentId,
                                DriverKey = item.DriverKey,
                                Email = email,
                                EmergencyContactName = item.EmergencyContactName,
                                EmergencyContactPhone = item.EmergencyContactPhone,
                                EmployeeId = item.EmployeeID,
                                EmployeeNumber = employeeNumber,
                                EmployeeStatusId = employeeStatusId,
                                EndDate = item.EndDate,
                                FirstName = item.FirstName,
                                IsAdministrator = item.IsAdministrator,
                                IsDriver = item.IsDriver,
                                IsHourly = item.IsHourly,
                                IsProjectManager = item.IsProjectManager,
                                IsRediMixVisible = item.IsRediMixVisible,
                                LastName = item.LastName,
                                LastUpdated = DateTime.Now,
                                LocationId = locationId,
                                LoginCount = 0,
                                Mobile = item.Mobile,
                                Phone = item.Phone,
                                PhoneNumber = item.Phone,
                                PhoneNumberConfirmed = true,
                                Rate = item.Rate,
                                ShiftEndTime = item.ShiftEndTime,
                                ShiftStartTime = item.ShiftStartTime,
                                SSN = item.SSN,
                                StartDate = item.StartDate,
                                State = string.IsNullOrEmpty(item.State) ? string.Empty : item.State.Trim(),
                                UserName = email,
                                VacationHours = item.VacationHours,
                                VehicleId = vehicleId,
                                YearsOfService = item.YearsOfService,
                                ZipCode = item.Zip,
                            };

                            IdentityResult result = await UserManager.CreateAsync(model, Membership.GeneratePassword(128, 18));
                            if (result.Succeeded)
                            {
                                imported++;
                            }
                            else
                            {
                                errors.AppendLine(item.EmployeeID + " failed to import: ");
                                foreach (var error in result.Errors)
                                {
                                    errors.AppendLine("     " + error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine(item.EmployeeID + " failed to import: " + ex.Message);
                        }
                    }
                }
            }
            return Content("Found " + found + ", imported " + imported + " Employees." + Environment.NewLine + errors.ToString(), "text/plain");
        }

        public ActionResult FixDefaultDrivers()
        {
            int i = 0;
            var vehicles = DataContext.Vehicles.ToList();
            foreach (var vehicle in vehicles)
            {
                if (!string.IsNullOrEmpty(vehicle.DefaultDriverId))
                {
                    int id = Convert.ToInt32(vehicle.DefaultDriverId);
                    vehicle.DefaultDriverId = GetEmployeeId(id);
                    DataContext.SaveChanges();
                    i++;
                }
            }
            return Content("Fixed " + i + " records.", "text/plain");
        }

        public ActionResult Vehicles()
        {
            int found = 0;
            int imported = 0;
            StringBuilder errors = new StringBuilder();
            using (OldDataManager.Models.OldDataManager context = new OldDataManager.Models.OldDataManager())
            {
                var items = context.Vehicles.ToList();
                found = items.Count();
                foreach (var item in items)
                {
                    Vehicle existing = DataContext.Vehicles.FirstOrDefault(x => x.OriginalId == item.VehicleID);
                    if (existing == null)
                    {
                        int? locationId = null;
                        if (item.LocationID.HasValue)
                        {
                            Location type = DataContext.Locations.FirstOrDefault(x => x.OriginalId.Value == item.LocationID);
                            if (type != null)
                            {
                                locationId = type.LocationId;
                            }
                        }

                        int? trailerTypeId = null;
                        if (item.LocationID.HasValue)
                        {
                            TrailerType type = DataContext.TrailerTypes.FirstOrDefault(x => x.OriginalId.Value == item.TrailerTypeID);
                            if (type != null)
                            {
                                trailerTypeId = type.TrailerTypeId;
                            }
                        }

                        int? vehicleTypeId = null;
                        if (item.LocationID.HasValue)
                        {
                            VehicleType type = DataContext.VehicleTypes.FirstOrDefault(x => x.OriginalId.Value == item.VehicleTypeID);
                            if (type != null)
                            {
                                vehicleTypeId = type.VehicleTypeId;
                            }
                        }

                        try
                        {
                            DataContext.Vehicles.Add(new Vehicle
                            {
                                Active = item.IsActive,
                                CurrentMeterReading = item.CurrentMeterReading,
                                DefaultDriverId = item.DefaultDriverID.ToString(),
                                EZPass = item.EZPass,
                                GrossAxleWeightRating = item.GrossAxleWeightRating,
                                GrossVehicleWeightRating = item.GrossVehicleWeightRating,
                                IsOnRoad = item.IsOnRoad,
                                LastGrease = item.LastGrease,
                                LastOil = item.LastOil,
                                LastService = item.LastService,
                                LastUpdated = DateTime.Now,
                                Length = item.Length,
                                Make = item.Make,
                                Name = item.VehicleName,
                                NextGrease = item.NextGrease,
                                NextOil = item.NextOil,
                                NextService = item.NextService,
                                Number = item.VehicleNumber,
                                NumberOfAxles = item.NumberOfAxles,
                                Registration = item.Registration,
                                RegistrationDate = item.RegistrationDate,
                                RegistrationExpiration = item.RegistrationExpiration,
                                ShouldExportUsage = item.ShouldExportUsage,
                                VehicleCode = item.VehicleCode,
                                VIN = item.VIN,
                                Weight = item.Weight,
                                Year = item.Year,
                                LocationId = locationId,
                                OriginalId = item.VehicleID,
                                TrailerTypeId = trailerTypeId,
                                VehicleTypeId = vehicleTypeId
                            });
                            DataContext.SaveChanges();
                            imported++;
                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine(item.VehicleTypeID + " error: " + ex.Message);
                        }
                    }
                }
            }
            return Content("Found " + found + ", imported " + imported + " Vehicles." + Environment.NewLine + errors.ToString(), "text/plain");
        }

        public ActionResult VehicleType()
        {
            int found = 0;
            int imported = 0;
            List<string> errors = new List<string>
            {
                Environment.NewLine
            };
            using (OldDataManager.Models.OldDataManager context = new OldDataManager.Models.OldDataManager())
            {
                var items = context.VehicleTypes.ToList();
                found = items.Count();
                foreach (var item in items)
                {
                    VehicleType existing = DataContext.VehicleTypes.FirstOrDefault(x => x.OriginalId == item.VehicleTypeID);
                    if (existing == null)
                    {
                        int? meterTypeID = null;
                        if (item.MeterTypeID.HasValue)
                        {
                            MeterType type = DataContext.MeterTypes.FirstOrDefault(x => x.OriginalId.Value == item.VehicleTypeID);
                            if (type != null)
                            {
                                meterTypeID = type.MeterTypeId;
                            }
                        }
                        try
                        {
                            DataContext.VehicleTypes.Add(new VehicleType
                            {
                                Active = true,
                                GreaseInterval = item.GreaseInterval,
                                IncludeInDispatch = item.IncludeInDispatch,
                                IncludeInReadyMixDispatch = item.IncludeInReadyMixDispatch,
                                MeterTypeId = meterTypeID,
                                Name = item.VehicleTypeName,
                                NeedsAnnualService = item.NeedsAnnualService,
                                NeedsGrease = item.NeedsGrease,
                                NeedsOil = item.NeedsOil,
                                OilInterval = item.OilInterval,
                                OriginalId = item.VehicleTypeID,
                                ServiceInterval = item.ServiceInterval,
                            });
                            DataContext.SaveChanges();
                            imported++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add(item.VehicleTypeID + " error: " + ex.Message);
                        }
                    }
                }
            }
            return Content("Found " + found + ", imported " + imported + " Vehicle Types.", "text/plain");
        }

    }
}