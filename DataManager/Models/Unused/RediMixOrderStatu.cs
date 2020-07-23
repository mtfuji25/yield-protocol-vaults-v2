using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RediMixOrderStatus")]
    public class RediMixOrderStatu
    {
        #region Methods
        public RediMixOrderStatu()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int OrderStatusID { get; set; }

        [DisplayName("Order Status Name"), Required, StringLength(50)]
        public string OrderStatusName { get; set; }

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
