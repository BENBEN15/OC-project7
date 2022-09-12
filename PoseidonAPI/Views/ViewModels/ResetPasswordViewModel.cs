using PoseidonAPI.Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace PoseidonAPI.Views.ViewModels
{
    public class ResetPasswordViewModel
    {

        public string emailError { get; set; }
        public string passwordError { get; set; }
        public string isValidated { get; set; }
        public string token { get; set; }

        public string Email { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
