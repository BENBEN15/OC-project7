namespace PoseidonAPI.Contracts.CurvePoint
{
    public record UpsertCurvePointRequest(
        int CurvePointId,
        byte? CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
