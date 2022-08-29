namespace PoseidonAPI.Contracts.User
{
    public record UserResponse(
        string id,
        string userName,
        string email,
        string phoneNumber
    );
}
