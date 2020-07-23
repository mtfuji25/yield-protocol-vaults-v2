using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("tblMeterTypes")]
    public class MeterType
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int MeterTypeId { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Name { get; set; }

        public int? OriginalId { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }

    [Table("tblTrailerTypes")]
    public class TrailerType
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int TrailerTypeId { get; set; }

        public int? OriginalId { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }

    [Table("tblVehicles")]
    public class Vehicle
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DisplayName("Default Driver")]
        public string DefaultDriverId { get; set; }

        [DataType(DataType.Text), MaxLength(64), StringLength(64)]
        public string EZPass { get; set;}

        [DisplayName("Current Meter Reading")]
        public int? CurrentMeterReading { get; set; }

        [DisplayName("Gross Axle Weight Rating")]
        public int? GrossAxleWeightRating { get; set; }

        [DisplayName("Gross Vehicle Weight Rating")]
        public int? GrossVehicleWeightRating { get; set; }

        [DisplayName("Is On Road")]
        public bool IsOnRoad { get; set; }

        [DisplayName("Last Grease")]
        public int? LastGrease { get; set; }

        [DisplayName("Last Oil")]
        public int? LastOil { get; set;  }

        [DataType(DataType.Date), DisplayName("Last Service")]
        public DateTime? LastService { get; set; }

        public DateTime LastUpdated { get; set; }

        public int? Length { get; set; }

        [DisplayName("Location")]
        public int? LocationId { get; set; }

        [DataType(DataType.Text), MaxLength(64), StringLength(64)]
        public string Make { get; set; }

        [DataType(DataType.Text), MaxLength(256), StringLength(256)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [DisplayName("Next Grease")]
        public int? NextGrease { get; set; }

        [DisplayName("Next Oil")]
        public int? NextOil { get; set; }

        [DataType(DataType.Date), DisplayName("Next Service")]
        public DateTime? NextService { get; set; }

        [DisplayName("Number Of Axles")]
        public int? NumberOfAxles { get; set; }

        public int? OriginalId { get; set; }

        [DataType(DataType.Text), MaxLength(64), StringLength(64)]
        public string Registration { get; set; }

        [DataType(DataType.Date), DisplayName("Registration Date")]
        public DateTime? RegistrationDate { get; set; }

        [DataType(DataType.Date), DisplayName("Registration Expires")]
        public DateTime? RegistrationExpiration { get; set; }

        [DisplayName("Should Export Usage")]
        public bool ShouldExportUsage { get; set; }

        [DisplayName("Trailer Type")]
        public int? TrailerTypeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int VehicleId { get; set; }

        [DataType(DataType.Text), DisplayName("Vehicle Code"), MaxLength(64), Required, StringLength(64)]
        public string VehicleCode { get; set; }

        [DisplayName("Vehicle Type")]
        public int? VehicleTypeId { get; set; }

        [DataType(DataType.Text), MaxLength(64), StringLength(64)]
        public string VIN { get; set; }

        public int? Weight { get; set; }

        public int? Year { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [DisplayName("Restore Point"), NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }
        #endregion

        #region One-to-One Relationships
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [ForeignKey("TrailerTypeId")]
        public TrailerType TrailerType { get; set; }

        [ForeignKey("VehicleTypeId")]
        public VehicleType VehicleType { get; set; }
        #endregion
    }

    [Table("tblVehicleTypes")]
    public class VehicleType
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DisplayName("Grease Interval")]
        public int? GreaseInterval { get; set; }

        [DisplayName("Include in Dispatch?")]
        public bool IncludeInDispatch { get; set; }

        [DisplayName("Include in Ready Mix Dispatch?")]
        public bool IncludeInReadyMixDispatch { get; set; }

        [DisplayName("Meter Type")]
        public int? MeterTypeId { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Name { get; set; }

        [DisplayName("Needs Annual Service?")]
        public bool NeedsAnnualService { get; set;  }

        [DisplayName("Needs Grease?")]
        public bool NeedsGrease { get; set; }

        [DisplayName("Needs Oil?")]
        public bool NeedsOil { get; set;  }

        [DisplayName("Oil Interval")]
        public int? OilInterval { get; set;  }

        public int? OriginalId { get; set; }

        [DisplayName("Service Interval")]
        public int? ServiceInterval { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int VehicleTypeId { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        #endregion

        #region One-to-One Relationships
        [ForeignKey("MeterTypeId")]
        public MeterType MeterType { get; set; }
        #endregion
    }
}