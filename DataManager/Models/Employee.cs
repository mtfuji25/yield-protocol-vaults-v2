using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Employees")]
    public class Employee
    {
        #region Methods
        public Employee()
        {
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int EmployeeID { get; set; }

        [DisplayName("Employee Number"), StringLength(50)]
        public string EmployeeNumber { get; set; }

        [DisplayName("Last Name"), Required, StringLength(50)]
        public string LastName { get; set; }

        [DisplayName("First Name"), Required, StringLength(50)]
        public string FirstName { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Mobile"), StringLength(50)]
        public string Mobile { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Phone"), StringLength(50)]
        public string Phone { get; set; }

        [DisplayName("Address"), StringLength(255)]
        public string Street { get; set; }

        [DisplayName("City"), StringLength(50)]
        public string City { get; set; }

        [DisplayName("State"), StringLength(50)]
        public string State { get; set; }

        [DisplayName("Zip Code"), StringLength(50)]
        public string Zip { get; set; }

        [DisplayName("SSN"), StringLength(50)]
        public string SSN { get; set; }

        [DataType(DataType.Date), DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Department")]
        public int? DepartmentID { get; set; }

        [DisplayName("Location")]
        public int? LocationID { get; set; }

        [DisplayName("Default Cost Center")]
        public int? DefaultCostCenterID { get; set; }

        [DataType(DataType.DateTime), DisplayName("Shift Start Time")]
        public DateTime? ShiftStartTime { get; set; }

        [DataType(DataType.DateTime), DisplayName("Shift End Time")]
        public DateTime? ShiftEndTime { get; set; }

        [DisplayName("Hourly")]
        public bool IsHourly { get; set; }

        [DisplayName("Driver")]
        public bool IsDriver { get; set; }

        [DisplayName("Project Manager")]
        public bool IsProjectManager { get; set; }

        [DisplayName("Employee Status")]
        public int? EmployeeStatusID { get; set; }

        [DisplayName("Start Date"),]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date), DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Vacation Hours")]
        public decimal? VacationHours { get; set; }

        [DisplayName("Years of Service")]
        public decimal? YearsOfService { get; set; }

        [DisplayName("Vehicle")]
        public int? VehicleID { get; set; }

        [Column(TypeName = "smallmoney"), DisplayName("Hourly Rate")]
        public decimal? Rate { get; set; }

        [DisplayName("Emergency Contact"), StringLength(200)]
        public string EmergencyContactName { get; set; }

        [DisplayName("Emergency Contact Phone"), StringLength(50)]
        public string EmergencyContactPhone { get; set; }

        public bool IsAdministrator { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string DriverKey { get; set; }

        public bool IsRediMixVisible { get; set; }

        public int? DefaultAssignmentTypeID { get; set; }

        public bool Active { get; set; } // IsActive

        public DateTime LastUpdated { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        [ForeignKey("")]
        public virtual EmployeeStatus EmployeeStatus { get; set; }

        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        public string Address
        {
            get
            {
                return string.Concat(FirstName, " ", LastName, Environment.NewLine, Street, Environment.NewLine, City, ", ", State, " ", Zip);
            }
        }

        [NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return string.Concat(FirstName, " ", LastName, Environment.NewLine, Street, Environment.NewLine, City, ", ", State, " ", Zip);
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Concat(FirstName, " ", LastName);
            }
        }

        [NotMapped]
        public string LastFirst
        {
            get
            {
                return string.Concat(LastName, ", ", FirstName);
            }
        }
        #endregion

        #region Collections
        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }

        public virtual ICollection<NonConformingReport> NonConformingReports { get; set; }

        public virtual ICollection<RediMixDriverAssignment> RediMixDriverAssignments { get; set; }

        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }

        public virtual ICollection<TimeCard> TimeCards { get; set; }

        public virtual ICollection<TimeClockEntry> TimeClockEntries { get; set; }

        public virtual ICollection<TimeClockPermission> TimeClockPermissions { get; set; }
        #endregion
    }
}
