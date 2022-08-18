namespace PoseidonAPI.Contracts.Rule
{
    public record CreateRuleRequest(
        string Name,
        string Description,
        string Json,
        string Template,
        string SqlStr,
        string SqlPart);
}
