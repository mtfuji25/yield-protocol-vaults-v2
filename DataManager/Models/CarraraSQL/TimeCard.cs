namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class TimeCard
    {
        #region Properties
        [HiddenInput]
        public int TimeCardID { get; set; }

        [DisplayName("Dept Code"), NotMapped]
        public string DeptCode
        {
            get
            {
                return Employee.Department.DepartmentCode;
            }
        }

        [DisplayName("Department"), NotMapped]
        public string Department
        {
            get
            {
                return Employee.Department.DepartmentName;
            }
        }

        [DisplayName("Employee")]
        public int EmployeeID { get; set; }

        [DisplayName("Status"), NotMapped]
        public string Status
        {
            get { return Employee.EmployeeStatus.EmployeeStatusName; }
        }

        [DisplayName("Hourly"), NotMapped]
        public bool Hourly
        {
            get { return Employee.IsHourly; }
        }

        [Column(TypeName = "numeric"), DisplayName("Regular")]
        public decimal? RegularHours { get; set; }

        [Column(TypeName = "numeric"), DisplayName("Overtime")]
        public decimal? OvertimeHours { get; set; }

        [Column(TypeName = "numeric"), DisplayName("Vacation")]
        public decimal? VacationHours { get; set; }

        [Column(TypeName = "numeric"), DisplayName("Holiday")]
        public decimal? HolidayHours { get; set; }

        [Column(TypeName = "numeric"), DisplayName("Total")]
        public decimal? TotalHours { get; set; }

        [DisplayName("Total Unaapproved"), NotMapped]
        public decimal? TotalUnaapproved
        {
            get
            {
                decimal regular = RegularHours ?? 0;
                decimal overtime = OvertimeHours ?? 0;
                decimal vacation = VacationHours ?? 0;
                decimal holiday = HolidayHours ?? 0;
                decimal total = TotalHours ?? 0;
                return total - regular - overtime - vacation - holiday;
            }
        }

        public DateTime? WeekEndDate { get; set; }

        public bool Assigned { get; set; }
        #endregion

        #region Foreign Keys
        public virtual Employee Employee { get; set; }
        #endregion
    }
}
