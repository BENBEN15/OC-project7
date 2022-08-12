using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public interface IService<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        T Save(T entity);
        void Update(T entityUpdate);
        void Delete(int id);
    }
}
