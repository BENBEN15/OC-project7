using FluentValidation;
using PoseidonAPI.Contracts.Rule;

namespace PoseidonAPI.Validators
{
    public class RuleCreateValidator : AbstractValidator<CreateRuleRequest>
    {
        public RuleCreateValidator()
        {

        }
    }

    public class RuleUpsertValidator : AbstractValidator<UpsertRuleRequest>
    {
        public RuleUpsertValidator()
        {

        }
    }
}
