namespace PoseidonAPI.Contracts.CurvePoint
{
    public record CurvePointResponse(
        int CurvePointId,
        byte? CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
