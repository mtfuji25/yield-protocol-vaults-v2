namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeClockPermissions")]
    public class TimeClockPermission
    {
        [Key]
        public int TimeClockPermissionsID { get; set; }

        public int EmployeeID { get; set; }

        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
