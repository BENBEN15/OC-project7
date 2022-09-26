using FluentValidation;
using PoseidonAPI.Contracts.Trade;

namespace PoseidonAPI.Validators
{
    public class TradeCreateValidator : AbstractValidator<CreateTradeRequest>
    {
        public TradeCreateValidator()
        {
            RuleFor(x => x.BuyQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Buy quantity is not a number");
            RuleFor(x => x.SellQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Sell quantity is not a number");
            RuleFor(x => x.BuyPrice).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Buy Price is not a number");
            RuleFor(x => x.SellPrice).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Sell price is not a number");
            RuleFor(x => x.TradeDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Trade date is not a valid Date");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
            RuleFor(x => x.RevisionDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Revision date is not a valid Date");
        }
    }

    public class TradeUpsertValidator : AbstractValidator<UpsertTradeRequest>
    {
        public TradeUpsertValidator()
        {
            RuleFor(x => x.BuyQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Buy quantity is not a number");
            RuleFor(x => x.SellQuantity).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Sell quantity is not a number");
            RuleFor(x => x.BuyPrice).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Buy Price is not a number");
            RuleFor(x => x.SellPrice).Must(x => double.TryParse(x.ToString(), out _)).WithMessage("Sell price is not a number");
            RuleFor(x => x.TradeDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Trade date is not a valid Date");
            RuleFor(x => x.CreationDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Creation date is not a valid Date");
            RuleFor(x => x.RevisionDate).Must(date => !date.Equals(default(DateTime))).WithMessage("Revision date is not a valid Date");
        }
    }
}
