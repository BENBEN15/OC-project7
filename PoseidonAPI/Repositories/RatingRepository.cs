using System.Collections.Generic;
using PoseidonAPI.Data;
using PoseidonAPI.Model;

namespace PoseidonAPI.Repositories
{
    public class RatingRepository : IRepository<Rating>
    {
        private readonly PoseidonDBContext _dbContext;

        public RatingRepository(PoseidonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Rating Get(int id)
        {
            Rating rating = _dbContext.Ratings.FirstOrDefault(x => x.RatingId == id);
            return rating;
        }

        public IEnumerable<Rating> GetAll()
        {
            IEnumerable<Rating> ratings = _dbContext.Ratings.Where(x => x.RatingId > 0);
            return ratings;
        }

        public Rating Save(Rating rating)
        {
            _dbContext.Add(rating);
            _dbContext.SaveChanges();
            return rating;
        }

        public void Update(Rating rating)
        {
            _dbContext.Ratings.Update(rating);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Rating rating = _dbContext.Ratings.FirstOrDefault(x => x.RatingId == id);
            _dbContext.Remove(rating);
            _dbContext.SaveChanges();
        }
    }
}
