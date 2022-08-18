namespace PoseidonAPI.Contracts.CurvePoint
{
    public record CreateCurvePointRequest(
        byte? CurveId,
        DateTime? AsOfDate,
        double? Term,
        double? Value,
        DateTime? CreationDate);
}
