namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TouchScreen
    {
        public int TouchScreenID { get; set; }

        [Required]
        [StringLength(100)]
        public string TouchScreenName { get; set; }

        public DateTime? LastSync { get; set; }
    }
}
