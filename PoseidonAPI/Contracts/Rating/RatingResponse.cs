namespace PoseidonAPI.Contracts.Rating
{
    public record RatingResponse(
        int RatingId,
        string MoodysRating,
        string SandPrating,
        string FitchRating,
        int OrderNumber);
}
