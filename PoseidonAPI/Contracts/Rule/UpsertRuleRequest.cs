namespace PoseidonAPI.Contracts.Rule
{
    public record UpsertRuleRequest(
        int RuleId,
        string Name,
        string Description,
        string Json,
        string Template,
        string SqlStr,
        string SqlPart);
}
