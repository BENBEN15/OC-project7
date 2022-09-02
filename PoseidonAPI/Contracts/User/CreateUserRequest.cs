namespace PoseidonAPI.Contracts.User
{
    public record CreateUserRequest(
        string Username,
        string Email,
        string Phonenumber,
        string Password,
        string ConfirmPassword);
}
