using FluentValidation;
using PoseidonAPI.Contracts.User;
using System.Text.RegularExpressions;

namespace PoseidonAPI.Validators
{
    public class UserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UserValidator()
        {
            RuleFor(request => request.userName).NotNull().WithMessage("You must specify an email");
            RuleFor(request => request.userName).NotEmpty().WithMessage("You must specify an email");
            RuleFor(request => request.email).Matches(@"^\S+@\S+\.\S+$").WithMessage("The email is not valid");
        }
    }
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(request => request.email).NotNull().WithMessage("You must specify an email");
            RuleFor(request => request.email).NotEmpty().WithMessage("You must specify an email");
            RuleFor(request => request.email).Matches(@"^\S+@\S+\.\S+$").WithMessage("The email is not valid");
        }
    }
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(request => request.email).NotNull().WithMessage("You must specify an email");
            RuleFor(request => request.email).NotEmpty().WithMessage("You must specify an email");
            RuleFor(request => request.email).Matches(@"^\S+@\S+\.\S+$").WithMessage("The email is not valid");
            RuleFor(request => request.newPassword).Equal(request => request.confirmNewPassword).WithMessage("The passwords are not identical");
            RuleFor(request => request.newPassword).Matches(@"^(?=.*[A-Z])$").WithMessage("The password should contain at least one capital letter");
            RuleFor(request => request.newPassword).Matches(@"^(?=.*\d)$").WithMessage("The password should contain at least one number");
            RuleFor(request => request.newPassword).Matches(@"^(?=.*[#$^+=!*()@%&])$").WithMessage("The password should contain at least one special character");
            RuleFor(request => request.newPassword).Matches(@"^.{8,}$").WithMessage("The password must contain at least 8 character");

        }
    }
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(request => request.Email).NotNull().WithMessage("You must specify an email");
            RuleFor(request => request.Email).NotEmpty().WithMessage("You must specify an email");
            RuleFor(request => request.Email).Matches(@"^\S+@\S+\.\S+$").WithMessage("The email is not valid");
            RuleFor(request => request.Username).NotNull().WithMessage("You must specify an username");
            RuleFor(request => request.Username).NotEmpty().WithMessage("You must specify an username");
            RuleFor(request => request.Password).NotNull().WithMessage("You must specify a password");
            RuleFor(request => request.Password).NotEmpty().WithMessage("You must specify a password");
            RuleFor(request => request.Password).Equal(request => request.ConfirmPassword).WithMessage("The passwords are not identical");
            RuleFor(request => request.Password).Matches(@"^.*[A-Z]").WithMessage("The password should contain at least one capital letter");
            RuleFor(request => request.Password).Matches(@"^.*\d$").WithMessage("The password should contain at least one number");
            RuleFor(request => request.Password).Matches(@"^.*[#$^+=!*()@%&]").WithMessage("The password should contain at least one special character");
            RuleFor(request => request.Password).Matches(@"^.{8,}$").WithMessage("The password must contain at least 8 character");
        }
    }
}
