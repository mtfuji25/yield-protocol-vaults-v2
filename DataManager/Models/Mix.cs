using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Mixes")]
    public class Mix
    {
        #region Methods
        public Mix()
        {
            IsActive = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int MixID { get; set; }

        [DisplayName("Mix Name"), Required, StringLength(50)]
        public string MixName { get; set; }

        [DisplayName("Mix Description"), StringLength(150)]
        public string MixDescription { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Bed> Beds { get; set; }

        public virtual ICollection<Pour> Pours { get; set; }

        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }
        #endregion
    }
}
