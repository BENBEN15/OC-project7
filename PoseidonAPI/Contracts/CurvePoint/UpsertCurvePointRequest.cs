namespace PoseidonAPI.Contracts.CurvePoint
{
    public record UpsertCurvePointRequest(
        int CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
