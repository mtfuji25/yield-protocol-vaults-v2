using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RediMixLoadTypes")]
    public class RediMixLoadType
    {
        #region Methods
        public RediMixLoadType()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RediMixLoadTypeID { get; set; }

        [DisplayName("RediMix Load Type Name"), Required, StringLength(50)]
        public string RediMixLoadTypeName { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }
        #endregion
    }
}
