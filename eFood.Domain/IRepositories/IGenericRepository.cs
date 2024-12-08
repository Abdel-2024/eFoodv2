
using System;
using System.Linq;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace eFood.Domain.IRepositories
{
    public interface IGenericRepository
    {
        Task<AppsListResult<T>> GetAll<T>() where T : class;
        Task<AppsListResult<T>> GetAll<T,U>() where T : class where U : class;
        Task<AppsListResult<T>> GetAll<T>(Pagination pagination,
                                        Expression<Func<T, bool>> search,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> order,
                                        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class;
        
        Task<AppsResult<T>> GetOne<T>(int id) where T : class;
        Task<AppsResult<T>> GetOne<T>(Expression<Func<T, bool>> search) where T : class;

        Task<AppsResult<T>> AddItem<T>(T entity, DbTransaction transaction=null) where T : class;
        Task<AppsResult<T>> UpdateItem<T>(T entity, DbTransaction transaction=null) where T : class;
        Task<AppsResult<T>> DeleteItem<T>(T entity, DbTransaction transaction=null) where T : class;

        Task<AppsListResult<T>> AddRangeItems<T>(IEnumerable<T> entities, 
                                                DbTransaction transaction = null) where T : class;
        Task<AppsListResult<T>> UpdateRangeItems<T>(IEnumerable<T> entities, 
                                                    DbTransaction transaction = null) where T : class;
        Task<AppsListResult<T>> DeleteRangeItems<T>(IEnumerable<T> entities, 
                                                    DbTransaction transaction = null) where T : class;

        Task<AppsResult> SaveChanges(DbTransaction transaction = null);

        string GetDBConnection();
    }
}
