using FluentValidation;
using PoseidonAPI.Contracts.Trade;

namespace PoseidonAPI.Validators
{
    public class TradeCreateValidator : AbstractValidator<CreateTradeRequest>
    {

    }

    public class TradeUpsertValidator : AbstractValidator<UpsertTradeRequest>
    {

    }
}
