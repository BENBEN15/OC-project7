using FluentValidation;
using PoseidonAPI.Contracts.CurvePoint;

namespace PoseidonAPI.Validators
{
    public class CurvePointCreateValidator : AbstractValidator<CreateCurvePointRequest>
    {
        public CurvePointCreateValidator()
        {
            RuleFor(x => x.CurveId).Must(x => int.TryParse(x.ToString(), out _)).WithMessage("Curve id is not a number");
            RuleFor(x => x.AsOfDate).Must(date => !date.Equals(default(DateTime))).WithMessage("As of date is not a number");
            RuleFor(x => x.Term).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Term is not a number");
            RuleFor(x => x.Value).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Value is not a number");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
        }
    }

    public class CurvePointUpsertValidator : AbstractValidator<UpsertCurvePointRequest>
    {
        public CurvePointUpsertValidator()
        {
            RuleFor(x => x.CurveId).Must(x => int.TryParse(x.ToString(), out _)).WithMessage("Curve id is not a number");
            RuleFor(x => x.AsOfDate).Must(date => !date.Equals(default(DateTime))).WithMessage("As of date is not a number");
            RuleFor(x => x.Term).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Term is not a number");
            RuleFor(x => x.Value).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Value is not a number");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
        }
    }
}
