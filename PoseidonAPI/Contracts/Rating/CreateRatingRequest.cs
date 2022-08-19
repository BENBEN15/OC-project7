namespace PoseidonAPI.Contracts.Rating
{
    public record CreateRatingRequest(
        string MoodysRating,
        string SandPrating,
        string FitchRating,
        int OrderNumber);
}
