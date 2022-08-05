using System.Collections.Generic;
using PoseidonAPI.Data;
using PoseidonAPI.Model;

namespace PoseidonAPI.Repositories
{
    public class CurvePointRepository : IRepository<CurvePoint>
    {
        private readonly PoseidonDBContext _dbContext;

        public CurvePointRepository(PoseidonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CurvePoint Get(int id)
        {
            CurvePoint curvePoint = _dbContext.CurvePoints.FirstOrDefault(x => x.CurvePointId == id);
            return curvePoint;
        }

        public IEnumerable<CurvePoint> GetAll()
        {
            IEnumerable<CurvePoint> curvePoints = _dbContext.CurvePoints.Where(x => x.CurvePointId > 0);
            return curvePoints;
        }

        public void Save(CurvePoint curvePoint)
        {
            _dbContext.Add(curvePoint);
            _dbContext.SaveChanges();
        }

        public void Update(CurvePoint curvePoint)
        {
            _dbContext.CurvePoints.Update(curvePoint);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            CurvePoint curvePoint = _dbContext.CurvePoints.FirstOrDefault(x => x.CurvePointId == id);
            _dbContext.Remove(curvePoint);
        }
    }
}
