namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Pour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pour()
        {
            NonConformingReports = new HashSet<NonConformingReport>();
            PourDetails = new HashSet<PourDetail>();
        }

        public int PourID { get; set; }

        [DataType(DataType.Date), DisplayName("Date/Time")]
        public DateTime PourDate { get; set; }

        [DisplayName("Bed")]
        public int BedID { get; set; }

        [DisplayName("Status")]
        public int PourStatusID { get; set; }

        public DateTime? PourTime { get; set; }

        [DisplayName("Mix")]
        public int? MixID { get; set; }

        [DisplayName("Slip #"), StringLength(50)]
        public string SlipNumber { get; set; }

        [DisplayName("Default Job")]
        public int? DefaultJobID { get; set; }

        [DisplayName("Default Mark Type")]
        public int? DefaultMarkTypeID { get; set; }

        [DisplayName("3/8\" Strands")]
        public int? ThreeInchStrands { get; set; }

        [DisplayName("0.5\" Strands")]
        public int? FiveInchStrands { get; set; }

        [DisplayName("0.6\" Strands")]
        public int? SixInchStrands { get; set; }

        [DisplayName("Jack #"), StringLength(100)]
        public string JackNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Release Test Date")]
        public DateTime? ReleaseTestDate { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Release Test 1")]
        public decimal? ReleaseTest1 { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Release Test 2")]
        public decimal? ReleaseTest2 { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Release Test Average")]
        public decimal? ReleaseTestAverage { get; set; }

        [DataType(DataType.Date)]
        [Column("28DayTestDate")]
        [DisplayName("28 Day Test Date")]
        public DateTime? C28DayTestDate { get; set; }

        [Column("28DayTest1", TypeName = "numeric")]
        [DisplayName("28 Day Test 1")]
        public decimal? C28DayTest1 { get; set; }

        [Column("28DayTest2", TypeName = "numeric")]
        [DisplayName("28 Day Test 2")]
        public decimal? C28DayTest2 { get; set; }

        [Column("28DayTestAverage", TypeName = "numeric")]
        [DisplayName("28 Day Test Average")]
        public decimal? C28DayTestAverage { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Other Test Date")]
        public DateTime? OtherTestDate { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Other Test 1")]
        public decimal? OtherTest1 { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Other Test 2")]
        public decimal? OtherTest2 { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Other Test Average")]
        public decimal? OtherTestAverage { get; set; }

        [DisplayName("# of Cylinders")]
        public int? NumberOfCylinders { get; set; }

        [StringLength(50)]
        public string Workability { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Thickness { get; set; }

        [DisplayName("Concrete (Yards)"), Column(TypeName = "numeric")]
        public decimal? YardsConcrete { get; set; }

        [DisplayName("Grout (Yards)"), Column(TypeName = "numeric")]
        public decimal? YardsGrout { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [DisplayName("Unit Weight"), Column(TypeName = "numeric")]
        public decimal? UnitWeight { get; set; }

        [DisplayName("Concrete Temp °F"), Column(TypeName = "numeric")]
        public decimal? ConcreteTemperature { get; set; }

        [DisplayName("Ambient Temp °F"), Column(TypeName = "numeric")]
        public decimal? AmbientTemperature { get; set; }

        [StringLength(250)]
        public string Weather { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Yield { get; set; }

        [DisplayName("Slump/Spred"), Column(TypeName = "numeric")]
        public decimal? Slump { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VSI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Air { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Release Strength")]
        public decimal? SpecReleaseStrength { get; set; }


        [Column(TypeName = "numeric")]
        [DisplayName("28 Day Strength")]
        public decimal? Spec28DayStrength { get; set; }

        [DisplayName("Max Slump")]
        [Column(TypeName = "numeric")]
        public decimal? SpecMaxSlump { get; set; }

        [DisplayName("Air")]
        [Column(TypeName = "numeric")]
        public decimal? SpecAir { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpecAirError { get; set; }

        [DisplayName("Notes")]
        [StringLength(250)]
        public string SpecNotes { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataType(DataType.MultilineText), DisplayName("Schedule Notes"),  Column(TypeName = "text")]
        public string PourScheduleNotes { get; set; }

        [DataType(DataType.MultilineText), Column(TypeName = "text")]
        public string Notes { get; set; }

        public virtual Bed Bed { get; set; }

        public virtual Job Job { get; set; }

        public virtual MarkType MarkType { get; set; }

        public virtual Mix Mix { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NonConformingReport> NonConformingReports { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PourDetail> PourDetails { get; set; }

        public virtual PourStatu PourStatu { get; set; }

        [NotMapped]
        public string Action { get; set; }
    }
}
