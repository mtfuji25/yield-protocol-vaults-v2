namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            Employees = new HashSet<Employee>();
            HoursAssignments = new HashSet<HoursAssignment>();
            HoursAssignments1 = new HashSet<HoursAssignment>();
            Loads = new HashSet<Load>();
            Loads1 = new HashSet<Load>();
            RediMixDriverAssignments = new HashSet<RediMixDriverAssignment>();
            RediMixLoads = new HashSet<RediMixLoad>();
            TimeClockEntries = new HashSet<TimeClockEntry>();
            VehicleMaintenances = new HashSet<VehicleMaintenance>();
        }

        public Employee DefaultDriver()
        {
            using (CarraraSQL context = new CarraraSQL())
            {
                return context.Employees.Find(DefaultDriverID);
            }
        }


        [HiddenInput]
        public int VehicleID { get; set; }

        [DisplayName("Vehicle Code"), Required, StringLength(50)]
        public string VehicleCode { get; set; }

        [DisplayName("Vehicle Name"), StringLength(255)]
        public string VehicleName { get; set; }

        [DisplayName("Vehicle Number"), StringLength(50)]
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

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string VehicleLabel
        {
            get
            {
                return VehicleCode + (string.IsNullOrEmpty(VehicleName) ? string.Empty : " " + VehicleName.Trim());
            }
        }
        #endregion

        public virtual Location Location { get; set; }

        public virtual TrailerType TrailerType { get; set; }

        public virtual VehicleType VehicleType { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoursAssignment> HoursAssignments1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixDriverAssignment> RediMixDriverAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeClockEntry> TimeClockEntries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleMaintenance> VehicleMaintenances { get; set; }
    }
}
