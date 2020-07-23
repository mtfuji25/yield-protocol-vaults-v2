using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataManager.Models
{
    [Table("ApplicationErrors")]
    public class ApplicationError
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApplicationErrorID { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        public DateTime? Date { get; set; }

        public string Source { get; set; }
        #endregion
    }
}
