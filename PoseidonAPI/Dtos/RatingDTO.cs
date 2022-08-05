namespace PoseidonAPI.Dtos
{
    public class RatingDTO
    {
        // TODO: Map columns in data table RATING with corresponding fields
        public int RatingId { get; set; }
        public string MoodysRating { get; set; }
        public string SandPrating { get; set; }
        public string FitchRating { get; set; }
        public byte? OrderNumber { get; set; }
    }
}