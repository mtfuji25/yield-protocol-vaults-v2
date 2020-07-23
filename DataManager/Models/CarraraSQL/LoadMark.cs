namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoadMark
    {
        public int LoadMarkID { get; set; }

        public int LoadID { get; set; }

        public int MarkID { get; set; }

        public int? Top { get; set; }

        public int? Left { get; set; }

        public virtual Load Load { get; set; }

        public virtual Mark Mark { get; set; }
    }
}
