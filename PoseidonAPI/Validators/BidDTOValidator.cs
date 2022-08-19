using FluentValidation;
using PoseidonAPI.Dtos;

namespace PoseidonAPI.Validators
{
    public class BidDTOValidator : AbstractValidator<BidDTO>
    {
        public BidDTOValidator()
        {
            RuleFor(bid => bid.Account).NotNull();
            RuleFor(bid => bid.Type).NotNull();
        }
    }
}
