using FluentValidation;
using PoseidonAPI.Contracts.Bid;

namespace PoseidonAPI.Validators
{
    public class BidCreateValidator : AbstractValidator<CreateBidRequest>
    {
        public BidCreateValidator()
        {
            RuleFor(x => x.Account).NotNull();
            RuleFor(x => x.Type).NotNull();
            RuleFor(x => x.BidQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid quantity is not a number");
        }
    }
    public class BidUpsertValidator : AbstractValidator<UpsertBidRequest>
    {
        public BidUpsertValidator()
        {
            RuleFor(x => x.Account).NotNull();
            RuleFor(x => x.Type).NotNull();
            RuleFor(x => x.BidQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid quantity is not a number");
        }
    }
}
