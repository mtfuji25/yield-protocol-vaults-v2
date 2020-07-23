using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("TouchScreens")]
    public class TouchScreen
    {
        #region Methods
        public TouchScreen()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int TouchScreenID { get; set; }

        [DisplayName("Touch Screen Name"), Required, StringLength(100)]
        public string TouchScreenName { get; set; }

        [DisplayName("IP Address")]
        public string IPAddress { get; set; }

        [DisplayName("Http Status")]
        public string HttpStatus { get; set; }

        [DataType(DataType.MultilineText), DisplayName("Ping Exception")]
        public string PingException { get; set; }

        [DisplayName("Last Status Check")]
        public DateTime? LastStatusCheck { get; set; }

        [DisplayName("Last Sync")]
        public DateTime? LastSync { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }
}
