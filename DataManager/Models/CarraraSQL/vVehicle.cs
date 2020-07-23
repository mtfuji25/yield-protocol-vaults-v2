namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vVehicle
    {
        [StringLength(50)]
        public string VehicleTypeName { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VehicleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string VehicleCode { get; set; }

        [StringLength(255)]
        public string VehicleName { get; set; }

        [StringLength(50)]
        public string VehicleNumber { get; set; }

        public int? NextGrease { get; set; }

        public int? CurrentMeterReading { get; set; }

        public int? LastGrease { get; set; }

        public int? LastOil { get; set; }

        public int? OilDue { get; set; }

        public int? GreaseDue { get; set; }

        public int? ServiceDue { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VehicleTypeID { get; set; }

        public int? DefaultDriverID { get; set; }

        public int? LocationID { get; set; }

        [StringLength(50)]
        public string EZPass { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsActive { get; set; }

        public int? Length { get; set; }

        [StringLength(50)]
        public string Make { get; set; }

        [StringLength(50)]
        public string Registration { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? RegistrationExpiration { get; set; }

        [StringLength(50)]
        public string VIN { get; set; }

        public int? Weight { get; set; }

        public int? Year { get; set; }

        public int? TrailerTypeID { get; set; }

        public int? NextOil { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool ShouldExportUsage { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsOnRoad { get; set; }

        public int? NumberOfAxles { get; set; }

        public int? GrossAxleWeightRating { get; set; }

        public int? GrossVehicleWeightRating { get; set; }

        [StringLength(50)]
        public string MeterTypeName { get; set; }

        public int? MeterTypeID { get; set; }

        public bool? NeedsAnnualService { get; set; }

        [StringLength(50)]
        public string LocationName { get; set; }

        public DateTime? NextService { get; set; }

        public DateTime? LastService { get; set; }

        public bool? NeedsOil { get; set; }

        public bool? NeedsGrease { get; set; }
    }
}
