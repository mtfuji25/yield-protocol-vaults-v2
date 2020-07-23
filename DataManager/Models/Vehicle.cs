using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        #region Methods
        public Vehicle()
        {
            IsActive = true;
        }

        public Employee DefaultDriver()
        {
            using (DataContext context = new DataContext())
            {
                return context.Employees.Find(DefaultDriverID);
            }
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int VehicleID { get; set; }

        [DisplayName("Vehicle Code"), Required, StringLength(50)]
        public string VehicleCode { get; set; }

        [DisplayName("Vehicle Name"), StringLength(255)]
        public string VehicleName { get; set; }

        [DisplayName("Account Number"), StringLength(50)]
        public string VehicleNumber { get; set; }

        [DisplayName("Vehicle Type")]
        public int VehicleTypeID { get; set; }

        [DisplayName("Driver")]
        public int? DefaultDriverID { get; set; }

        [DisplayName("Location")]
        public int? LocationID { get; set; }

        [DisplayName("EZPass"), StringLength(50)]
        public string EZPass { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Length")]
        public int? Length { get; set; }

        [DisplayName("Make"), StringLength(50)]
        public string Make { get; set; }

        [DisplayName("Registration"), StringLength(50)]
        public string Registration { get; set; }

        [DataType(DataType.Date), DisplayName("Registration Date")]
        public DateTime? RegistrationDate { get; set; }

        [DataType(DataType.Date), DisplayName("Registration Expires")]
        public DateTime? RegistrationExpiration { get; set; }

        [DisplayName("VIN"), StringLength(50)]
        public string VIN { get; set; }

        [DisplayName("Weight")]
        public int? Weight { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }

        [DisplayName("Trailer Type")]
        public int? TrailerTypeID { get; set; }

        [DisplayName("Next Oil")]
        public int? NextOil { get; set; }

        [DisplayName("Next Grease")]
        public int? NextGrease { get; set; }

        [DataType(DataType.Date), DisplayName("Next Service")]
        public DateTime? NextService { get; set; }

        [DisplayName("Current Meter Reading")]
        public int? CurrentMeterReading { get; set; }

        [DisplayName("Last Oil")]
        public int? LastOil { get; set; }

        [DisplayName("Last Grease")]
        public int? LastGrease { get; set; }

        [DataType(DataType.Date), DisplayName("Last Service")]
        public DateTime? LastService { get; set; }

        [DisplayName("Should Export Usage")]
        public bool ShouldExportUsage { get; set; }

        [DisplayName("Is On Road")]
        public bool IsOnRoad { get; set; }

        [DisplayName("Number Of Axles")]
        public int? NumberOfAxles { get; set; }

        [DisplayName("Gross Axle Weight Rating")]
        public int? GrossAxleWeightRating { get; set; }

        [DisplayName("Gross Vehicle Weight Rating")]
        public int? GrossVehicleWeightRating { get; set; }

        public DateTime LastUpdated { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [ForeignKey("TrailerTypeID")]
        public virtual TrailerType TrailerType { get; set; }

        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleType { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }

        [NotMapped]
        public string VehicleLabel
        {
            get
            {
                return VehicleCode + (string.IsNullOrEmpty(VehicleName) ? string.Empty : " " + VehicleName.Trim());
            }
        }
        #endregion

        #region Collections
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        public virtual ICollection<HoursAssignment> HoursAssignments1 { get; set; }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<Load> Loads1 { get; set; }

        public virtual ICollection<RediMixDriverAssignment> RediMixDriverAssignments { get; set; }

        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }

        public virtual ICollection<TimeClockEntry> TimeClockEntries { get; set; }

        public virtual ICollection<VehicleMaintenance> VehicleMaintenances { get; set; }
        #endregion
    }
}
