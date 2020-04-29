using System.Collections.Generic;

namespace SocialNetwork.Services
{
    public interface IService<T, TId>
    {
        T Create(T obj);
        List<T> Read();
        T Read(TId id);
        T Update(T obj, TId id);
        void Delete(T obj);
        void Delete(TId id);

    }
}