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
    public interface IUnitOfWork
    {
        void SaveChanges(out AppsResult result);
        void SaveChanges<T>(T entity,out AppsResult<T> result) where T : class;
        void SaveChanges<T>(IEnumerable<T> entities, out AppsListResult<T> result) where T : class;

        Task SaveChangesAsync<T>(T entity, out AppsResult<T> result) where T : class;
        Task SaveChangesAsync<T>(IEnumerable<T> entities, out AppsListResult<T> result) where T : class;   

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        void GetAll<T>(out IEnumerable<T> resultList) where T : class;
        void GetAll<T>(Pagination pagination,   
                        Expression<Func<T, bool>> search,
                        Func<IQueryable<T>, IOrderedQueryable<T>> order,
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
                        out IEnumerable<T> resultList) where T : class;

        Task<AppsListResult<T>> GettAllAsync<T>(
                        IQueryable<T> query,
                        Expression<Func<T, bool>> search,
                        Pagination pagination) where T : class;

        IQueryable<T> GetQueryable<T>() where T : class;
        IQueryable<T> GetQueryable<T>(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class;

        void GetOne<T>(int id, out T entity) where T : class;
        void GetOne<T>(Expression<Func<T, bool>> predicate, out T entity ) where T : class;

        Task GetOneAsync<T>(int id, out AppsResult<T> result) where T : class;
        Task GetOneAsync<T>(Expression<Func<T, bool>> search, out AppsResult<T> result) where T : class;

        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;

        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
    }
}
