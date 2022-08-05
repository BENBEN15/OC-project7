using System.Collections.Generic;
using PoseidonAPI.Data;
using PoseidonAPI.Model;

namespace PoseidonAPI.Repositories
{
    public class BidRepository : IRepository<Bid>
    {
        private readonly PoseidonDBContext _dbContext;

        public BidRepository(PoseidonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Bid Get(int id)
        {
            Bid bid = _dbContext.Bids.FirstOrDefault(x => x.BidId == id);
            return bid;
        }

        public IEnumerable<Bid> GetAll()
        {
            IEnumerable<Bid> bids = _dbContext.Bids.Where(x => x.BidId > 0);
            return bids;
        }

        public void Save(Bid bid)
        {
            _dbContext.Add(bid);
            _dbContext.SaveChanges();
        }

        public void Update(Bid bidsUpdate)
        {
            _dbContext.Bids.Update(bidsUpdate);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Bid bid = _dbContext.Bids.FirstOrDefault(x => x.BidId == id);
            _dbContext.Remove(bid);
        }
    }
}
