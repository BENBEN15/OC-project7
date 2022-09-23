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
            //Transform(from: bid => bid.BidQuantity, to: value => double.TryParse(value, out double val) ? (int?)val : null);
            //RuleFor(bid => bid.BidQuantity).Must(bid => );
        }
    }
}
