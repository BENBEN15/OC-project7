namespace PoseidonAPI.Contracts.CurvePoint
{
    public record CreateCurvePointRequest(
        int CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
