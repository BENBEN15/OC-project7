using FluentValidation;
using PoseidonAPI.Contracts.User;

namespace PoseidonAPI.Validators
{
    public class UserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UserValidator()
        {
            RuleFor(request => request.userName).NotNull();
            RuleFor(request => request.userName).NotEmpty();
            RuleFor(request => request.email).EmailAddress();
        }
    }
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(request => request.email).NotNull();
            RuleFor(request => request.email).NotEmpty();
            RuleFor(request => request.email).EmailAddress();
        }
    }
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(request => request.email).NotNull();
            RuleFor(request => request.email).NotEmpty();
            RuleFor(request => request.email).EmailAddress();
            RuleFor(request => request.newPassword).Equal(request => request.confirmNewPassword);
        }
    }
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(request => request.Email).NotNull();
            RuleFor(request => request.Email).NotEmpty();
            RuleFor(request => request.Email).EmailAddress();
            RuleFor(request => request.Username).NotNull();
            RuleFor(request => request.Username).NotEmpty();
            RuleFor(request => request.Username).NotEmpty();
            RuleFor(request => request.Password).NotNull();
            RuleFor(request => request.Password).NotEmpty();
            RuleFor(request => request.Password).Equal(request => request.ConfirmPassword);
        }
    }
}
