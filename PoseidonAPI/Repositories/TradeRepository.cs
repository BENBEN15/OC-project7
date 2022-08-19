using System.Collections.Generic;
using PoseidonAPI.Data;
using PoseidonAPI.Model;

namespace PoseidonAPI.Repositories
{
    public class TradeRepository : IRepository<Trade>
    {
        private readonly PoseidonDBContext _dbContext;

        public TradeRepository(PoseidonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Trade Get(int id)
        {
            Trade trade = _dbContext.Trades.FirstOrDefault(x => x.TradeId == id);
            return trade;
        }

        public IEnumerable<Trade> GetAll()
        {
            IEnumerable<Trade> trades = _dbContext.Trades.Where(x => x.TradeId > 0);
            return trades;
        }

        public Trade Save(Trade trade)
        {
            _dbContext.Add(trade);
            _dbContext.SaveChanges();
            return trade;
        }

        public void Update(Trade trade)
        {
            _dbContext.Trades.Update(trade);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Trade trade = _dbContext.Trades.FirstOrDefault(x => x.TradeId == id);
            _dbContext.Remove(trade);
            _dbContext.SaveChanges();
        }
    }
}
