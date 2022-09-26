using FluentValidation;
using PoseidonAPI.Contracts.Bid;

namespace PoseidonAPI.Validators
{
    public class BidCreateValidator : AbstractValidator<CreateBidRequest>
    {
        public BidCreateValidator()
        {
            RuleFor(x => x.BidQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid quantity is not a number");
            RuleFor(x => x.AskQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Ask quantity is not a number");
            RuleFor(x => x.BidValue).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid value is not a number");
            RuleFor(x => x.Ask).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Ask is not a number");
            RuleFor(x => x.BidDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Bid date is not a valid Date");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
            RuleFor(x => x.RevisionDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Revision date is not a valid Date");
        }
    }
    public class BidUpsertValidator : AbstractValidator<UpsertBidRequest>
    {
        public BidUpsertValidator()
        {
            RuleFor(x => x.BidQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid quantity is not a number");
            RuleFor(x => x.AskQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Ask quantity is not a number");
            RuleFor(x => x.BidValue).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Bid value is not a number");
            RuleFor(x => x.Ask).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Ask is not a number");
            RuleFor(x => x.BidDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Bid date is not a valid Date");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
            RuleFor(x => x.RevisionDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Revision date is not a valid Date");
        }
    }
}
