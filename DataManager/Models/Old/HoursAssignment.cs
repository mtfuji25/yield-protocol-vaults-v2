namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoursAssignments")]
    public class HoursAssignment
    {
        public int HoursAssignmentID { get; set; }

        public int AssignmentTypeID { get; set; }

        public DateTime DateWorked { get; set; }

        public int? DepartmentID { get; set; }

        public int? DrivenVehicleID { get; set; }

        public int EmployeeID { get; set; }

        public int? JobID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal OvertimeHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RegularHours { get; set; }

        public int? MaintainedVehicleID { get; set; }

        public virtual Department Department { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public virtual Vehicle Vehicle1 { get; set; }
    }
}
