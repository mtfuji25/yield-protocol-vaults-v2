namespace DataManager.Models.CarraraSQL
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("RouteMileage")]
    public partial class RouteMileage
    {
        #region Properties
        [HiddenInput]
        public int RouteMileageID { get; set; }

        [DisplayName("Route")]
        public int RouteID { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Mileage { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        public virtual Route Route { get; set; }

        public virtual State State { get; set; }
        #endregion
    }
}
