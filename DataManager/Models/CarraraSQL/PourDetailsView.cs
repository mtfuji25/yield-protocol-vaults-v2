namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PourDetailsView")]
    public partial class PourDetailsView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MarkID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MarkNumber { get; set; }

        [StringLength(100)]
        public string MarkDescription { get; set; }

        public int? MarkTypeID { get; set; }

        public int? JobID { get; set; }

        [StringLength(50)]
        public string JobNumber { get; set; }

        [StringLength(50)]
        public string MarkTypeName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Thickness { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SquareFeet { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PourDetailID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PourID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quantity { get; set; }

        public int? MarkSizeTypeID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalSquareFeet { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalWeight { get; set; }
    }
}
