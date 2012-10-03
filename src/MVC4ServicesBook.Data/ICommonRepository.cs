using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MVC4ServicesBook.Data
{
    public interface ICommonRepository
    {
        T Get<T>(object id);
        T Load<T>(object id);
        void Save<T>(T obj);
        void Save<T>(T obj, bool flush);
        void Delete(object obj);
        T Get<T>(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll<T>();
        IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate);
    }
}
