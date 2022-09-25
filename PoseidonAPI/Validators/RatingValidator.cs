using FluentValidation;
using PoseidonAPI.Contracts.Rating;

namespace PoseidonAPI.Validators
{
    public class RatingCreateValidator : AbstractValidator<CreateRatingRequest>
    {

    }

    public class RatingUpsertValidator : AbstractValidator<UpsertRatingRequest>
    {

    }
}