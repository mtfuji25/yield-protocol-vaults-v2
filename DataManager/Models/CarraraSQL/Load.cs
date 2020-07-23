namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Load
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Load()
        {
            LoadMarks = new HashSet<LoadMark>();
        }

        [HiddenInput]
        public int LoadID { get; set; }

        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Delivery Time")]
        public DateTime? DeliveryTime { get; set; }

        [DisplayName("Departure Date")]
        public DateTime? SiteDepartureDate { get; set; }

        [DisplayName("Departure Time")]
        public DateTime? SiteDepartureTime { get; set; }
        [DisplayName("Carrara Departure Date")]
        public DateTime? CarraraDepartureDate { get; set; }

        [DisplayName("Carrara Departure Time")]
        public DateTime? CarraraDepartureTime { get; set; }

        [DisplayName("Driver")]
        public int? DriverID { get; set; }

        [DisplayName("Diver Mobile"), NotMapped]
        public string DiverMobile
        {
            get { return DriverID.HasValue ? Employee.Mobile : string.Empty; }
        }

        [DisplayName("Diver Phone"), NotMapped]
        public string DriverPhone
        {
            get { return DriverID.HasValue ? Employee.Phone : string.Empty; }
        }

        [DisplayName("Diver Seniority"), NotMapped]
        public string DriverSeniority
        {
            get { return string.Empty; }
        }

        [DisplayName("Ind. Driver")]
        public int? IndependentDriverID { get; set; }

        [DisplayName("Ind. Diver Mobile"), NotMapped]
        public string IndependentDiverMobile
        {
            get { return IndependentDriverID.HasValue ? Contact.MobilePhone : string.Empty; }
        }

        [DisplayName("Ind. Diver Phone"), NotMapped]
        public string IndependentDriverPhone
        {
            get { return IndependentDriverID.HasValue ? Contact.HomePhone : string.Empty; }
        }

        [DisplayName("Ind. Diver Work Phone"), NotMapped]
        public string IndependentDriverWorkPhone
        {
            get { return IndependentDriverID.HasValue ? Contact.WorkPhone : string.Empty; }

        }

        [DataType(DataType.MultilineText), DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("Contacted")]
        public bool HasDriverBeenContacted { get; set; }

        [DisplayName("#")]
        public int? LoadNumber { get; set; }

        [DisplayName("Vehicle")]
        public int? VehicleID { get; set; }

        [DisplayName("Vehicle"), NotMapped]
        public string VehicleCode
        {
            get { return VehicleID.HasValue ? Vehicle.VehicleCode : string.Empty; }
        }

        [DisplayName("Job")]
        public int? JobID { get; set; }

        [DisplayName("Job"), NotMapped]
        public string JobNumber
        {
            get { return JobID.HasValue ? Job.JobNumber : string.Empty; }
        }

        [DisplayName("Job Name"), NotMapped]
        public string JobName
        {
            get { return JobID.HasValue ? Job.JobName : string.Empty; }
        }

        [DisplayName("Loaded")]
        public bool IsLoaded { get; set; }

        [DisplayName("Status")]
        public int? LoadStatusID { get; set; }

        [DisplayName("Trailer")]
        public int? TrailerID { get; set; }


        public bool IsDropLoad { get; set; }


        public int? JobSiteLocationID { get; set; }

        public int? LoadTypeID { get; set; }

        [StringLength(255)]
        public string MarkList { get; set; }

        public int? RequiredTrailerTypeID { get; set; }

        public DateTime? SiteArrivalDate { get; set; }

        public DateTime? SiteArrivalTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalMarkWeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalWeight { get; set; }


        public string DriverNotes { get; set; }

        public string MarkLayout { get; set; }

        [DisplayName("Department")]
        public int? DepartmentID { get; set; }

        public int? AssignmentTypeID { get; set; }

        public bool IsQuickLoad { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public int? AltSequenceNumber { get; set; }

        [DisplayName("Cement Hauling")]
        public int? RouteID { get; set; }




        public virtual Contact Contact { get; set; }

        public virtual Department Department { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        public virtual JobSiteLocation JobSiteLocation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoadMark> LoadMarks { get; set; }

        public virtual LoadStatu LoadStatu { get; set; }

        public virtual LoadType LoadType { get; set; }

        public virtual Route Route { get; set; }

        public virtual TrailerType TrailerType { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public virtual Vehicle Vehicle1 { get; set; }
    }
}
