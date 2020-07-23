using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DataManager.Models
{
    [Table("Pours")]
    public class Pour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pour()
        {
            NonConformingReports = new HashSet<NonConformingReport>();
            PourDetails = new HashSet<PourDetail>();
        }

        public int PourID { get; set; }

        [DisplayName("Pour Date")]
        public DateTime PourDate { get; set; }

        [DisplayName("Pour Status")]
        public int PourStatusID { get; set; }

        [DisplayName("Pour Time")]
        public DateTime? PourTime { get; set; }

        [DisplayName("Mix")]
        public int? MixID { get; set; }

        [DisplayName("Bed")]
        public int BedID { get; set; }

        [DisplayName("Thickness"), Column(TypeName = "numeric")]
        public decimal? Thickness { get; set; }

        [DisplayName("Concrete"), Column(TypeName = "numeric")]
        public decimal? YardsConcrete { get; set; }

        [DisplayName("Grout"), Column(TypeName = "numeric")]
        public decimal? YardsGrout { get; set; }

        [DisplayName("3/8")]
        public int? ThreeInchStrands { get; set; }

        [DisplayName("0.5")]
        public int? FiveInchStrands { get; set; }

        [DisplayName("0.6")]
        public int? SixInchStrands { get; set; }

        [DisplayName("Slump/Spread"), Column(TypeName = "numeric")]
        public decimal? Slump { get; set; }

        [DisplayName("Unit Weight"), Column(TypeName = "numeric")]
        public decimal? UnitWeight { get; set; }

        [DisplayName("Concrete Temp"), Column(TypeName = "numeric")]
        public decimal? ConcreteTemperature { get; set; }

        [DisplayName("Yield"), Column(TypeName = "numeric")]
        public decimal? Yield { get; set; }

        [DisplayName("Release Test 1"), Column(TypeName = "numeric")]
        public decimal? ReleaseTest1 { get; set; }

        [DisplayName("Release Test 2"), Column(TypeName = "numeric")]
        public decimal? ReleaseTest2 { get; set; }

        [DisplayName("Release Test Avg"), Column(TypeName = "numeric")]
        public decimal? ReleaseTestAverage { get; set; }

        public DateTime? ReleaseTestDate { get; set; }

        [DisplayName("28-Day Test 1"), Column("28DayTest1", TypeName = "numeric")]
        public decimal? C28DayTest1 { get; set; }

        [DisplayName("28-Day Test 2"), Column("28DayTest2", TypeName = "numeric")]
        public decimal? C28DayTest2 { get; set; }

        [DisplayName("28-Day Test Avg"), Column("28DayTestAverage", TypeName = "numeric")]
        public decimal? C28DayTestAverage { get; set; }

        [Column("28DayTestDate")]
        public DateTime? C28DayTestDate { get; set; }

        [DisplayName("Other Test 1"), Column(TypeName = "numeric")]
        public decimal? OtherTest1 { get; set; }

        [DisplayName("Other Test 2"), Column(TypeName = "numeric")]
        public decimal? OtherTest2 { get; set; }

        [DisplayName("Other Test Avg"), Column(TypeName = "numeric")]
        public decimal? OtherTestAverage { get; set; }

        [DisplayName("Other Test Date")]
        public DateTime? OtherTestDate { get; set; }




        [StringLength(50)]
        public string SlipNumber { get; set; }

        public int? DefaultJobID { get; set; }

        public int? DefaultMarkTypeID { get; set; }

        [StringLength(100)]
        public string JackNumber { get; set; }

        public int? NumberOfCylinders { get; set; }

        [StringLength(50)]
        public string Workability { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmbientTemperature { get; set; }

        [StringLength(250)]
        public string Weather { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VSI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Air { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpecReleaseStrength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Spec28DayStrength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpecMaxSlump { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpecAir { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpecAirError { get; set; }

        [StringLength(250)]
        public string SpecNotes { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Column(TypeName = "text")]
        public string PourScheduleNotes { get; set; }

        [Column(TypeName = "text")]
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
    }
}
