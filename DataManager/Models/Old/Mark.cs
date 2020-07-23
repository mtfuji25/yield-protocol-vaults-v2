namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Marks")]
    public class Mark
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mark()
        {
            LoadMarks = new HashSet<LoadMark>();
            PourDetails = new HashSet<PourDetail>();
        }

        public int MarkID { get; set; }

        [Required]
        [StringLength(50)]
        public string MarkNumber { get; set; }

        [StringLength(100)]
        public string MarkDescription { get; set; }

        public int? MarkTypeID { get; set; }

        public int JobID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        public int? RequiredQuantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Thickness { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SquareFeet { get; set; }

        public bool IsDrawn { get; set; }

        public bool IsReleased { get; set; }

        public DateTime? DateDrawn { get; set; }

        public DateTime? DateReleased { get; set; }

        [StringLength(150)]
        public string Location { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Job Job { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoadMark> LoadMarks { get; set; }

        public virtual MarkType MarkType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PourDetail> PourDetails { get; set; }
    }
}
