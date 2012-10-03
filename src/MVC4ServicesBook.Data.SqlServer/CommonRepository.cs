using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace MVC4ServicesBook.Data.SqlServer
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ISession _session;

        public CommonRepository(ISession session)
        {
            _session = session;
        }

        public T Get<T>(object id)
        {
            return _session.Get<T>(id);
        }

        public T Load<T>(object id)
        {
            return _session.Load<T>(id);
        }

        public void Save<T>(T obj)
        {
            _session.SaveOrUpdate(obj);
        }

        public void Save<T>(T obj, bool flush)
        {
            _session.SaveOrUpdate(obj);
            _session.Flush();
        }

        public void Delete(object obj)
        {
            _session.Delete(obj);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _session.Query<T>().ToList();
        }

        public IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().Where(predicate).ToList();
        }
    }
}
