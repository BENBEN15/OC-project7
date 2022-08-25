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
        }
    }
}
