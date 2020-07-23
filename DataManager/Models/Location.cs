using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Locations")]
    public class Location
    {
        #region Methods
        public Location()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int LocationID { get; set; }

        [DisplayName("Location Name"), Required, StringLength(50)]
        public string LocationName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        #endregion
    }
}
