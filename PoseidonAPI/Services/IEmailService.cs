namespace PoseidonAPI.Services
{
    public interface IEmailService
    {
        Task<bool> sendResetPasswordEmail(string token, string toEmail);
    }
}
