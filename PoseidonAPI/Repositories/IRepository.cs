using System.Collections.Generic;

namespace PoseidonAPI.Repositories
{
    public interface IRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Save(T entity);
        void Update(T entityUpdate);
        void Delete(int id);
    }
}
