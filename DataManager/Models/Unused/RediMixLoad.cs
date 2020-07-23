using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RediMixLoads")]
    public class RediMixLoad
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RediMixLoadID { get; set; }

        [DisplayName("Order ID")]
        public int? RediMixOrderID { get; set; }

        [DisplayName("Load Number")]
        public int? LoadNumber { get; set; }

        [DisplayName("Vehicle")]
        public int? VehicleID { get; set; }

        [DisplayName("Driver")]
        public int? DriverID { get; set; }

        [DisplayName("Load Type")]
        public int? RediMixLoadTypeID { get; set; }

        [DisplayName("Start Time")]
        public DateTime? StartTime { get; set; }

        [DisplayName("End Time")]
        public DateTime? EndTime { get; set; }

        [DisplayName("Yards Concrete")]
        public decimal? YardsConcrete { get; set; }

        [DisplayName("Short Load?")]
        public bool IsShortLoad { get; set; }

        [DisplayName("Ten One")]
        public bool TenOne { get; set; }

        [DisplayName("Ten One Time")]
        public DateTime? TenOneTime { get; set; }

        [DisplayName("Ten Two")]
        public bool TenTwo { get; set; }

        [DisplayName("Ten Two Time")]
        public DateTime? TenTwoTime { get; set; }

        [DisplayName("Ten Three")]
        public bool TenThree { get; set; }

        [DisplayName("Ten Three Time")]
        public DateTime? TenThreeTime { get; set; }

        [DisplayName("Ten Nine")]
        public bool TenNine { get; set; }

        [DisplayName("Ten Nine Time")]
        public DateTime? TenNineTime { get; set; }

        [Column(TypeName = "timestamp"), MaxLength(8), Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("DriverID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("RediMixLoadTypeID")]
        public virtual RediMixLoadType RediMixLoadType { get; set; }

        [ForeignKey("RediMixOrderID")]
        public virtual RediMixOrder RediMixOrder { get; set; }

        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }
        #endregion
    }
}
