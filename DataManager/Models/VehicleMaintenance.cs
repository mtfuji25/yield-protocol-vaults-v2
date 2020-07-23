using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("VehicleMaintenance")]
    public class VehicleMaintenance
    {
        #region Methods
        public VehicleMaintenance()
        {
            DateOfService = DateTime.Now;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int VehicleMaintenanceID { get; set; }

        [DisplayName("Vehicle")]
        public int VehicleID { get; set; }

        [DataType(DataType.Date), DisplayName("Date Of Service")]
        public DateTime DateOfService { get; set; }

        public int? Meter { get; set; }

        public bool Grease { get; set; }

        public bool Service { get; set; }

        [DisplayName("Fuel Amount")]
        public decimal? FuelAmount { get; set; }

        public bool? Oil { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }
        #endregion
    }
}
