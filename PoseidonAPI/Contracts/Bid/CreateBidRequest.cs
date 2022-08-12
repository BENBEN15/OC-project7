namespace PoseidonAPI.Contracts.Bid
{
    public record CreateBidRequest(
        string Account,
        string Type,
        double? BidQuantity,
        double? AskQuantity,
        double? Bid,
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
        string SourceListId ,
        string Side);
}
