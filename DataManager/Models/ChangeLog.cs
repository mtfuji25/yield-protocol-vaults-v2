using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("ChangeLogs")]
    public class ChangeLog
    {
        #region Properties
        public DateTime DateChanged { get; set; }

        public string EntityName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int Id { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string PrimaryKeyValue { get; set; }

        public string PropertyName { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }
        #endregion
    }
}