using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataManager.Models
{
    [Table("TimeCards")]
    public class TimeCard
    {
        #region Properties
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

        [DisplayName("Regular")]
        public decimal? RegularHours { get; set; }

        [DisplayName("Overtime")]
        public decimal? OvertimeHours { get; set; }

        [DisplayName("Vacation")]
        public decimal? VacationHours { get; set; }

        [DisplayName("Holiday")]
        public decimal? HolidayHours { get; set; }

        [DisplayName("Total")]
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

        #region Properties
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
        #endregion
    }
}
/*
Dept Code 		50532
Department		Precast palnt
Employee		Name
Status			Active
Hourly			Checkbox
Regular			40.00
Overtime		7.00
Vacation		0
Holiday			0
Total			47.00
Total Approved	  	47.00
Total Unaapproved	0
Total Absent Hours	0
*/
