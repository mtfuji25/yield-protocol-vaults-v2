using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("RouteMileage")]
    public class RouteMileage
    {
        public RouteMileage()
        {
            Active = true;
        }

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RouteMileageID { get; set; }

        [DisplayName("Route")]
        public int RouteID { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }

        public decimal Mileage { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }

        [ForeignKey("StateID")]
        public virtual State State { get; set; }
        #endregion
    }
}
