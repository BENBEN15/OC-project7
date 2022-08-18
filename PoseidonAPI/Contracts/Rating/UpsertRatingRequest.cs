namespace PoseidonAPI.Contracts.Rating
{
    public record UpsertRatingRequest(
        int RatingId,
        string MoodysRating,
        string SandPrating,
        string FitchRating,
        byte? OrderNumber);
}
