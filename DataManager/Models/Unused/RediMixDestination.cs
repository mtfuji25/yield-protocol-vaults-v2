using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{

    [Table("RediMixDestinations")]
    public class RediMixDestination
    {
        #region Methods
        public RediMixDestination()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RediMixDestinationID { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [DisplayName("Zip Code"), StringLength(50)]
        public string ZipCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [DisplayName("NY Miles From Middlebury")]
        public decimal? NYMilesFromMiddlebury { get; set; }

        [DisplayName("VT Miles From Middlebury")]
        public decimal? VTMilesFromMiddlebury { get; set; }

        [DisplayName("NY Miles From Rutland")]
        public decimal? NYMilesFromRutland { get; set; }

        [DisplayName("VT Miles From Rutland")]
        public decimal? VTMilesFromRutland { get; set; }

        [DisplayName("NY Miles From Crown Point")]
        public decimal? NYMilesFromCrownPoint { get; set; }

        [DisplayName("VT Miles From Crown Point")]
        public decimal? VTMilesFromCrownPoint { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }
        #endregion
    }
}
