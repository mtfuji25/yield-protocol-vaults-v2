namespace DataManager.Models
{
    public class PourDetailView
    {
        #region Properties
        public int JobID { get; set; }
        public string MarkNumber { get; set; }

        public int Quantity { get; set; }
        public string MarkRange { get; set; }
        public decimal? Camber { get; set; }

        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Thickness { get; set; }
        public int? MarkTypeID { get; set; }
        public decimal? Weight { get; set; }
        public decimal? SquareFeet { get; set; }

        public int MarkID { get; set; }
        public int PourDetailID { get; set; }
        public int PourID { get; set; }
        #endregion
    }
}