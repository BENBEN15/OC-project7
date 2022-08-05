using System.Collections.Generic;
using PoseidonAPI.Data;
using PoseidonAPI.Model;

namespace PoseidonAPI.Repositories
{
    public class RuleRepository : IRepository<Rule>
    {
        private readonly PoseidonDBContext _dbContext;

        public RuleRepository(PoseidonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Rule Get(int id)
        {
            Rule rule = _dbContext.Rules.FirstOrDefault(x => x.RuleId == id);
            return rule;
        }

        public IEnumerable<Rule> GetAll()
        {
            IEnumerable<Rule> rules = _dbContext.Rules.Where(x => x.RuleId > 0);
            return rules;
        }

        public void Save(Rule rule)
        {
            _dbContext.Add(rule);
            _dbContext.SaveChanges();
        }

        public void Update(Rule rule)
        {
            _dbContext.Rules.Update(rule);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Rule rule = _dbContext.Rules.FirstOrDefault(x => x.RuleId == id);
            _dbContext.Remove(rule);
        }
    }
}
