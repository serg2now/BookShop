using BookShop.Api.DAL.DataContext;
using BookShop.Api.DAL.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookShop.Api.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IModel
    {
        private BookShopContext _dbContext;
        private DbSet<T> _table;

        public Repository(BookShopContext bookShopContext)
        {
            _dbContext = bookShopContext;
            _table = _dbContext.Set<T>();
        }

        public async Task<int> AddItemAsync(T item)
        {
            _table.Add(item);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string includes = "")
        {
            var fieldsToInclude = includes.Split(',').Where(s => s.Length > 0);

            var query = _table.AsQueryable();

            if (fieldsToInclude.Count() > 0)
            {
                foreach (var field in fieldsToInclude)
                {
                    query = query.Include(field);
                }
            }

            return await query.ToArrayAsync();
        }

        public async Task<T> GetItemByIdAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> FindItemAsync(Expression<Func<T, bool>> filter)
        {
            var query = _table.AsQueryable();

            query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveItemAsync(T item)
        {
            _table.Remove(item);

            await _dbContext.SaveChangesAsync();
        }
    }
}
