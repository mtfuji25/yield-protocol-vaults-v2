namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RediMixDriverAssignment
    {
        public int RediMixDriverAssignmentID { get; set; }

        public DateTime Date { get; set; }

        public int VehicleID { get; set; }

        public int? DriverID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
