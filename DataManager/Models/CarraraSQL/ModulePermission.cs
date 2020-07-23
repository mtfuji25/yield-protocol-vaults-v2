namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ModulePermission
    {
        [Key]
        public int ModulePermissionsID { get; set; }

        public int EmployeeID { get; set; }

        public int ModuleID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Module Module { get; set; }
    }
}
