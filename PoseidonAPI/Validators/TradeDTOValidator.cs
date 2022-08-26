using FluentValidation;
using PoseidonAPI.Dtos;

namespace PoseidonAPI.Validators
{
    public class TradeDTOValidator : AbstractValidator<TradeDTO>
    {
        public TradeDTOValidator()
        {
            RuleFor(trade => trade.Account).NotNull();
            RuleFor(trade => trade.Type).NotNull();
        }
    }
}
