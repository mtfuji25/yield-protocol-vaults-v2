namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TimeClockEntry
    {
        #region Properties
        public int TimeClockEntryID { get; set; }

        [NotMapped]
        public string Department
        {
            get
            {
                if (Employee == null)
                {
                    return string.Empty;
                }
                return Employee.Department.DepartmentName;
            }
        }

        [NotMapped]
        public int? DepartmentId
        {
            get
            {
                if (Employee == null)
                {
                    return null;
                }
                return Employee.DepartmentID;
            }
        }

        [DisplayName("Employee")]
        public int EmployeeID { get; set; }

        [DisplayName("Start Time"), NotMapped]
        public DateTime? StartTime
        {
            get
            {
                return Employee.ShiftStartTime;
            }
        }

        [DisplayName("Day Of Week"), NotMapped]
        public string DayOfWeek
        {
            get { return ClockIn.HasValue ? ClockIn.Value.DayOfWeek.ToString() : string.Empty; }
        }

        [DisplayName("Clock In")]
        public DateTime? ClockIn { get; set; }

        [NotMapped]
        public DateTime? ClockInDate { get; set; }

        [NotMapped]
        public DateTime? ClockInTime { get; set; }

        [DisplayName("Original Clock In")]
        public DateTime? OriginalClockIn { get; set; }

        [DisplayName("Lunch")]
        public bool Lunch { get; set; }

        [DisplayName("Clock Out")]
        public DateTime? ClockOut { get; set; }

        [NotMapped]
        public DateTime? ClockOutDate { get; set; }

        [NotMapped]
        public DateTime? ClockOutTime { get; set; }

        [DisplayName("Original Clock Out")]
        public DateTime? OriginalClockOut { get; set; }

        [Column(TypeName = "numeric"), DisplayName("Hours Worked")]
        public decimal? HoursWorked { get; set; }

        [DisplayName("Cost Center")]
        public int? CostCenterID { get; set; }

        [DisplayName("Vehicle Driven")]
        public int? DrivenVehicleID { get; set; }

        [DisplayName("Approved"), NotMapped]
        public bool Approved
        {
            get { return ApprovedDateTime.HasValue; }
        }

        [DisplayName("Approved On")]
        public DateTime? ApprovedDateTime { get; set; }

        [DisplayName("Approved By"), StringLength(50)]
        public string ApprovedBy { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("Status"), NotMapped]
        public string Status
        {
            get
            {
                List<string> issues = new List<string>();
                if (!CostCenterID.HasValue)
                {
                    issues.Add("No Cost Center");
                }
                if (!ClockIn.HasValue || !ClockOut.HasValue)
                {
                    issues.Add("Incomplete");
                }
                if (Employee.ShiftStartTime.HasValue && ClockIn.HasValue)
                {
                    string converted = ClockIn.Value.ToString("MM/dd/yy") + " " + Employee.ShiftStartTime.Value.ToString("h:mm:ss tt");
                    DateTime convertedDate = Convert.ToDateTime(converted);
                    if (convertedDate > ClockIn.Value)
                    {
                        issues.Add("Early Start");
                    }
                }
                return string.Join(",", issues);
            }
        }

        public int? AssignmentType { get; set; }
        #endregion

        #region Foreign Keys
        public virtual Employee Employee { get; set; }

        public virtual Vehicle Vehicle { get; set; }
        #endregion
    }

    public class TimeClockEntryView
    {
        public string Department { get; set; }
        public decimal? HoursWorked { get; set; }
        public IEnumerable<TimeClockEntry> TimeClockEntries { get; set; }
    }
}
