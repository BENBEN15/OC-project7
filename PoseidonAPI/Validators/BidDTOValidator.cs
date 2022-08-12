using FluentValidation;
using PoseidonAPI.Dtos;

namespace PoseidonAPI.Validators
{
    public class BidDTOValidator : AbstractValidator<BidDTO>
    {
        public BidDTOValidator()
        {
            //RuleFor().NotEqual(0);
        }
    }
}
