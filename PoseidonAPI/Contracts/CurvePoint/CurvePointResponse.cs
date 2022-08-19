namespace PoseidonAPI.Contracts.CurvePoint
{
    public record CurvePointResponse(
        int CurvePointId,
        int CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
