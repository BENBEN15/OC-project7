namespace PoseidonAPI.Contracts.Rule
{
    public record UpsertRuleRequest(
        string Name,
        string Description,
        string Json,
        string Template,
        string SqlStr,
        string SqlPart);
}
