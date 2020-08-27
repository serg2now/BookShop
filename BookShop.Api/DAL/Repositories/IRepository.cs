using BookShop.Api.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookShop.Api.DAL.Repositories
{
    public interface IRepository<T> where T: IModel 
    {
        Task<IEnumerable<T>> GetItemsAsync(string includes = "");

        Task<T> FindItemAsync(Expression<Func<T, bool>> filter);

        Task<T> GetItemByIdAsync(Guid id);

        Task<int> AddItemAsync(T item);

        Task RemoveItemAsync(T item);
    }
}
