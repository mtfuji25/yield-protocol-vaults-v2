namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PourDetails")]
    public class PourDetail
    {
        public int PourDetailID { get; set; }

        public int PourID { get; set; }

        public int MarkID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Camber { get; set; }

        [StringLength(50)]
        public string MarkRange { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Mark Mark { get; set; }

        public virtual Pour Pour { get; set; }
    }
}
