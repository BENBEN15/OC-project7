namespace PoseidonAPI.Contracts.User
{
    public record UpdateUserRequest(
        string userName,
        string newUserName,
        string email,
        string phoneNumber);
}
