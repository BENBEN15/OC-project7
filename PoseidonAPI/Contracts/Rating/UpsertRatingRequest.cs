namespace PoseidonAPI.Contracts.Rating
{
    public record UpsertRatingRequest(
        string MoodysRating,
        string SandPrating,
        string FitchRating,
        int OrderNumber);
}
