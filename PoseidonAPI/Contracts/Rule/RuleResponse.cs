namespace PoseidonAPI.Contracts.Rule
{
    public record RuleResponse(
        int RuleId,
        string Name,
        string Description,
        string Json,
        string Template,
        string SqlStr,
        string SqlPart);
}
