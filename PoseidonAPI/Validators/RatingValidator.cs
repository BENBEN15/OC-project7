using FluentValidation;
using PoseidonAPI.Contracts.Rating;

namespace PoseidonAPI.Validators
{
    public class RatingCreateValidator : AbstractValidator<CreateRatingRequest>
    {
        public RatingCreateValidator()
        {
            RuleFor(x => x.OrderNumber).Must(x => int.TryParse(x.ToString(), out _)).WithMessage("Order number id is not a number");
        }
    }

    public class RatingUpsertValidator : AbstractValidator<UpsertRatingRequest>
    {
        public RatingUpsertValidator()
        {
            RuleFor(x => x.OrderNumber).Must(x => int.TryParse(x.ToString(), out _)).WithMessage("Order number id is not a number");
        }
    }
}