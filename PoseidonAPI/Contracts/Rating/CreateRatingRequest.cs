namespace PoseidonAPI.Contracts.Rating
{
    public record CreateRatingRequest(
        string MoodysRating,
        string SandPrating,
        string FitchRating,
        byte? OrderNumber);
}
