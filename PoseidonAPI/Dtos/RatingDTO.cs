namespace PoseidonAPI.Dtos
{
    public class RatingDTO
    {
        public int RatingId { get; set; }
        public string MoodysRating { get; set; }
        public string SandPrating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }
    }
}