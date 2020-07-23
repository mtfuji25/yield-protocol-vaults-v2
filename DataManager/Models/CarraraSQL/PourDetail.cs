namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PourDetail
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

        [DisplayName("Default Job"), NotMapped]
        public int? DefaultJobID { get; set; }

        [DisplayName("Default Mark Type"), NotMapped]
        public int? DefaultMarkTypeID { get; set; }

        public Mark GetMark()
        {
            if (PourDetailID == 0)
            {
                return new Mark
                {
                    JobID = DefaultJobID.Value,
                    MarkTypeID = DefaultMarkTypeID
                };
            }
            using (CarraraSQL context = new CarraraSQL())
            {
                return context.Marks.Find(MarkID);
            }
        }

    }
}
