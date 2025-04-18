using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<(IEnumerable<T>, int)> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes);
        Task<T?> FindOneAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes);
        Task InsertAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(object id);
        void DeleteAsync(T entity);
	}
}
