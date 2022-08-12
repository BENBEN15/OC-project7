namespace PoseidonAPI.Contracts.Bid
{
    public record BidResponse(
        int BidId,
        string Account,
        string Type,
        double? BidQuantity,
        double? AskQuantity,
        double? BidValue,
        double? Ask,
        string Benchmark,
        DateTime? BidDate,
        string Commentary,
        string Security,
        string Status,
        string Trader,
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
