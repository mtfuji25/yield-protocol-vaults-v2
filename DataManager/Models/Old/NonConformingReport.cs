namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NonConformingReports")]
    public class NonConformingReport
    {
        public int NonConformingReportID { get; set; }

        public int PourID { get; set; }

        public int JobID { get; set; }

        public DateTime? DateChecked { get; set; }

        [StringLength(50)]
        public string MarkNumber { get; set; }

        [StringLength(100)]
        public string Finding { get; set; }

        [StringLength(100)]
        public string Action { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public int? CompletedByID { get; set; }

        public DateTime? CompletedOn { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        public virtual Pour Pour { get; set; }
    }
}
