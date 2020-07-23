namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApplicationError
    {
        public int ApplicationErrorID { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        public DateTime? Date { get; set; }

        public string Source { get; set; }
    }
}
