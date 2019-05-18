using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public interface IRepository
    {
        Task Insert<T>(T item, CancellationToken cancellationToken);
        Task Update<T>(T item, CancellationToken cancellationToken);
        T Find<T>(Expression<Func<T, bool>> predicate);
        IEnumerable<T> All<T>();
    }
}
