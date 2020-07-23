using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("MeterTypes")]
    public class MeterType
    {
        #region Methods
        public MeterType()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int MeterTypeID { get; set; }

        [DisplayName("Meter Type Name"), Required, StringLength(50)]
        public string MeterTypeName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<VehicleType> VehicleTypes { get; set; }
        #endregion
    }
}
