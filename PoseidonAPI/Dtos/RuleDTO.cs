namespace PoseidonAPI.Dtos
{
    public class RuleDTO
    {
        // TODO: Map columns in data table RULENAME with corresponding fields
        public int RuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
        public string Template { get; set; }
        public string SqlStr { get; set; }
        public string SqlPart { get; set; }
    }
}