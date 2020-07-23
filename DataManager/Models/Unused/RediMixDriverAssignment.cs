using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RediMixDriverAssignments")]
    public class RediMixDriverAssignment
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RediMixDriverAssignmentID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Vehicle")]
        public int VehicleID { get; set; }

        [DisplayName("Driver")]
        public int? DriverID { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("DriverID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }
        #endregion
    }
}
