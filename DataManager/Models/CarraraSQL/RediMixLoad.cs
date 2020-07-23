namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RediMixLoad
    {
        public int RediMixLoadID { get; set; }

        public int? RediMixOrderID { get; set; }

        public int? LoadNumber { get; set; }

        public int? VehicleID { get; set; }

        public int? DriverID { get; set; }

        public int? RediMixLoadTypeID { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? YardsConcrete { get; set; }

        public bool IsShortLoad { get; set; }

        public bool TenOne { get; set; }

        public DateTime? TenOneTime { get; set; }

        public bool TenTwo { get; set; }

        public DateTime? TenTwoTime { get; set; }

        public bool TenThree { get; set; }

        public DateTime? TenThreeTime { get; set; }

        public bool TenNine { get; set; }

        public DateTime? TenNineTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual RediMixLoadType RediMixLoadType { get; set; }

        public virtual RediMixOrder RediMixOrder { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
