namespace PoseidonAPI.Contracts.User
{
    public record ResetPasswordRequest(
        string email,
        string newPassword,
        string confirmNewPassword,
        string token
        );
}
