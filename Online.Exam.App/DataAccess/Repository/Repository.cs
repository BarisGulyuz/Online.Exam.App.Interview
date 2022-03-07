using Microsoft.EntityFrameworkCore;
using Online.Exam.App.DataAccess.Context;
using Online.Exam.App.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Online.Exam.App.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ExamContext _context;

        public Repository(ExamContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return await (filter == null ? _context.Set<T>().CountAsync() : _context.Set<T>().CountAsync(filter));
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null) query = query.Where(filter);

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null) query = query.Where(filter);

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(T exEntity, T newEntity)
        {
            //_context.Update(entity);
            //await _context.SaveChangesAsync();

            _context.Entry(exEntity).CurrentValues.SetValues(newEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T Entity)
        {
            _context.Update(Entity);
            await _context.SaveChangesAsync();
        }
    }
}
