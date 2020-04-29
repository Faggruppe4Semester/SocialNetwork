using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SocialNetwork.Services
{
    public interface IService<T, TId>
    {
        /*----- Default CRUD-Implementations -----*/
        T Create(T obj);
        List<T> Read();
        T Read(TId id);
        T Update(T obj, TId id);
        void Delete(T obj);
        void Delete(TId id);

        /*----- Custom RUD-Implementation -----*/ //They let the user decide which filter to search with.
        /*
         * Example:
         * int id = 0;
         * _userService.Read(user => user.Id == id); //Returns the user with the initialized id.
         */
        public T Read(Expression<Func<T, bool>> filter);
        public T Update(T obj, Expression<Func<T, bool>> filter);
        public void Delete(Expression<Func<T, bool>> filter);

    }
}
