using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        #region Methods
        public VehicleType()
        {
            Active = true;

        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int VehicleTypeID { get; set; }

        [DisplayName("Vehicle Type Name"), Required, StringLength(50)]
        public string VehicleTypeName { get; set; }

        [DisplayName("Meter Type")]
        public int? MeterTypeID { get; set; }

        [DisplayName("Grease Interval")]
        public int? GreaseInterval { get; set; }

        [DisplayName("Oil Interval")]
        public int? OilInterval { get; set; }

        [DisplayName("Service Interval")]
        public int? ServiceInterval { get; set; }

        [DisplayName("Include in Ready Mix Dispatch?")]
        public bool IncludeInReadyMixDispatch { get; set; }

        [DisplayName("Include in Dispatch?")]
        public bool IncludeInDispatch { get; set; }

        [DisplayName("Needs Annual Service?")]
        public bool NeedsAnnualService { get; set; }

        [DisplayName("Needs Grease?")]
        public bool NeedsGrease { get; set; }

        [DisplayName("Needs Oil?")]
        public bool NeedsOil { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("MeterTypeID")]
        public virtual MeterType MeterType { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        #endregion
    }
}
