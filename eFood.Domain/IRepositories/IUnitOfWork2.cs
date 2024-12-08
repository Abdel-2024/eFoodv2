using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.IRepositories
{
    public interface IUnitOfWork2
    {
        void SaveChanges(out AppsResult result);
        void SaveChanges<T>(T entity, out AppsResult<T> result) where T : class;
        void SaveChanges<T>(IEnumerable<T> entities, out AppsListResult<T> result ) where T : class;

        Task SaveChangesAsync(out AppsResult result);   
        Task SaveChangesAsync<T>(T entity, out AppsResult<T> result) where T : class;       
        Task SaveChangesAsync<T>(IEnumerable<T> entities, out AppsListResult<T> result) where T : class;

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        IQueryable<T> GetQueryable<T>() where T : class;
        IQueryable<T> GetQueryable<T>(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class;    

        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<T> GettAll<T>(
                                Pagination pagination,
                                Expression<Func<T, bool>> search,
                                Func<IQueryable<T>, IOrderedQueryable<T>> order,
                                Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class;
        IEnumerable<T> GettAll<T>(
                                IQueryable<T> query,
                                Pagination pagination,
                                Func<IQueryable<T>, IOrderedQueryable<T>> order) where T : class;

        Task<AppsListResult<T>> GetAllAsync<T>() where T : class;
        Task<AppsListResult<T>> GettAllAsync<T>(
                                Pagination pagination,
                                Expression<Func<T, bool>> search,
                                Func<IQueryable<T>, IOrderedQueryable<T>> order,
                                Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class;
        Task<AppsListResult<T>> GettAllAsync<T>(    
                                IQueryable<T> query,
                                Pagination pagination,
                                Func<IQueryable<T>, IOrderedQueryable<T>> order) where T : class;

        T GetOne<T>(int id) where T : class;
        T GetOne<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<AppsResult<T>> GetOneAsync<T>(int id) where T : class; 
        Task<AppsResult<T>> GetOneAsync<T>(Expression<Func<T, bool>> search) where T : class;   

        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;

        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
    }
}
