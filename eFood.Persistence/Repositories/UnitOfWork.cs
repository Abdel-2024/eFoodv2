using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Persistence.Context;
using eFood.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly eFoodContext _db;
        private bool _disposed;

        public UnitOfWork(eFoodContext db)
        {
            _db = db;
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Set<T>().Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _db.Set<T>().AddRange(entities);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }

        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void GetAll<T>(out IEnumerable<T> resultList) where T : class
        {
            throw new NotImplementedException();
        }

        public void GetOne<T>(int id, out T entity) where T : class
        {
            entity = _db.Set<T>().Find(id);
        }

        public void GetOne<T>(Expression<Func<T, bool>> predicate, out T entity) where T : class
        {        
            entity = _db.Set<T>().Where(predicate).FirstOrDefault();
        }

        public void Remove<T>(T entity) where T : class
        {
            _db.Set<T>().Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            _db.Set<T>().RemoveRange(entities);
        }

        public void SaveChanges(out AppsResult result)
        {
            result = new AppsResult();

            try
            {
                _db.SaveChanges();

                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }
        }

        public void SaveChanges<T>(T entity, out AppsResult<T> result) where T : class
        {
            result = new AppsResult<T>();

            try
            {
                if (_db.SaveChanges() < 0)
                {
                    result.Entity = null;
                }

                result.Success = true;
                result.Entity = entity;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Entity = null;

                result.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }
        }

        public void SaveChanges<T>(IEnumerable<T> entities, out AppsListResult<T> result) where T : class
        {
            result = new AppsListResult<T>();

            try
            {
                var numberRecords = _db.SaveChanges();

                if (numberRecords < 0)
                {
                    result.Entities = null;
                }

                result.Success = true;
                result.Entities = entities;
                result.TotalRecords = numberRecords;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Entities = null;
                result.TotalRecords = -1;

                result.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }
        }

        public void GetAll<T>(Pagination pagination,    
                                        Expression<Func<T, bool>> search, 
                                        Func<IQueryable<T>, IOrderedQueryable<T>> order, 
                                        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
                                        out IEnumerable<T> AppsListResult) where T : class
        {
            AppsListResult = new List<T>();

            var queryable = _db.Set<T>().AsQueryable();

            if (!object.Equals(search, null))
            {
                queryable = queryable.Where(search);
            }

            if (!object.Equals(order, null))
            {
                queryable = order(queryable);
            }

            if (!object.Equals(includes, null))
            {
                queryable = includes(queryable);
            }

            try
            {
                if (pagination.Page == -1)
                {
                    AppsListResult =  queryable.ToList();
                }
                else
                {
                    AppsListResult = queryable.Paginate(pagination).ToList();
                }
            }
            catch (Exception ex)
            {
                AppsListResult = null;
            }
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return _db.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetQueryable<T>(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class
        {
            var queryable = _db.Set<T>().AsQueryable();

            if (!object.Equals(includes, null))
            {
                queryable = includes(queryable);
            }

            return queryable;
        }

        public async Task<AppsListResult<T>> GettAllAsync<T>(IQueryable<T> queryable,
                                                        Expression<Func<T, bool>> search,
                                                        Pagination pagination) where T : class
        {
            var AppsListResult = new AppsListResult<T>();

            if (!object.Equals(search, null))
            {
                queryable = queryable.Where(search);
            }

            try
            {
                if (pagination.Page == -1)
                {
                    AppsListResult.Entities = await queryable.ToListAsync();
                }
                else
                {
                    AppsListResult.Entities = await queryable.Paginate(pagination).ToListAsync();
                }

                AppsListResult.Success = true;
                AppsListResult.TotalRecords = queryable.Count();
            }
            catch (Exception ex)
            {
                AppsListResult.Success = false;
                AppsListResult.Entities = null;

                AppsListResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult<AppsListResult<T>>(AppsListResult);
        }

        public async Task<AppsResult<T>> GetOneAsync<T>(int id) where T : class
        {
            var AppsResult = new AppsResult<T>();

            var result = await _db.Set<T>().FindAsync(id);

            if (Object.Equals(result, null))
            {
                AppsResult.Success = false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = "Entity not found" } };
            }

            AppsResult.Success = true;
            AppsResult.Entity = result;

            return await Task.FromResult<AppsResult<T>>(AppsResult);
        }

        public async Task<AppsResult<T>> GetOneAsync<T>(Expression<Func<T, bool>> search) where T : class
        {
            var AppsResult = new AppsResult<T>();

            var result = await _db.Set<T>().Where(search).FirstOrDefaultAsync();

            if (Object.Equals(result, null))
            {
                AppsResult.Success = false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = "Entity not found" } };
            }

            AppsResult.Success = true;
            AppsResult.Entity = result;

            return await Task.FromResult<AppsResult<T>>(AppsResult);
        }

        public Task SaveChangesAsync<T>(T entity, out AppsResult<T> result) where T : class
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync<T>(IEnumerable<T> entities, out AppsListResult<T> result) where T : class
        {
            throw new NotImplementedException();
        }

        public Task GetOneAsync<T>(int id, out AppsResult<T> result) where T : class
        {
            throw new NotImplementedException();
        }

        public Task GetOneAsync<T>(Expression<Func<T, bool>> search, out AppsResult<T> result) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
