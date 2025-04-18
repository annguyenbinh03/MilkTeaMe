using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<(IEnumerable<T>, int)> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return (await query.ToListAsync(), totalItems);
        }
        public async Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            // Get primary key name from EF Core metadata
            var keyName = _context.Model
                                  .FindEntityType(typeof(T))?
                                  .FindPrimaryKey()?
                                  .Properties
                                  .Select(x => x.Name)
                                  .FirstOrDefault();
            if (keyName == null)
                throw new InvalidOperationException($"Can not find primary key for {typeof(T).Name}");

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, keyName) == id);
        }

        public async Task<T?> FindOneAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

		public void DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
		}

		private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand)
            {
                return operand.Member.Name;
            }
            throw new InvalidOperationException("Invalid expression format");
        }
	}
}
