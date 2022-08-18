namespace PoseidonAPI.Contracts.Trade
{
    public record UpsertTradeRequest(
        int TradeId,
        string Account,
        string Type,
        double? BuyQuantity,
        double? SellQuantity,
        double? BuyPrice,
        double? SellPrice,
        DateTime? TradeDate,
        string Security,
        string Status,
        string Trader,
        string Benchmark,
        string Book,
        string CreationName,
        DateTime? CreationDate,
        string RevisionName,
        DateTime? RevisionDate,
        string DealName,
        string DealType,
        string SourceListId,
        string Side);
}
