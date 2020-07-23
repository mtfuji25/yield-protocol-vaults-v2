using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RediMixOrders")]
    public class RediMixOrder
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RediMixOrderID { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [DisplayName("Destination")]
        public int? RediMixDestinationID { get; set; }

        [DisplayName("Location")]
        public int? LocationID { get; set; }

        [DisplayName("Order Status")]
        public int? OrderStatusID { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Start Time")]
        public DateTime? StartTime { get; set; }

        public decimal? Duration { get; set; }

        [DisplayName("Yards Concrete")]
        public decimal? YardsConcrete { get; set; }

        [DisplayName("Mix")]
        public int? MixID { get; set; }

        [DisplayName("Number Of Loads")]
        public int? NumberOfLoads { get; set; }

        [Column(TypeName = "timestamp"), MaxLength(8), Timestamp]
        public byte[] RowVersion { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public bool IsComplete { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        public string RediMixOrderLabel
        {
            get
            {
                return string.Concat(RediMixOrderID, ": ", Customer, " ", Location);
            }
        }
        #endregion

        #region Foreign Keys
        [ForeignKey("LocationID")]
        public virtual Location Location1 { get; set; }

        [ForeignKey("MixID")]
        public virtual Mix Mix { get; set; }

        [ForeignKey("RediMixDestinationID")]
        public virtual RediMixDestination RediMixDestination { get; set; }

        [ForeignKey("OrderStatusID")]
        public virtual RediMixOrderStatu RediMixOrderStatu { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }
        #endregion
    }
}
