using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Persistence.Context;
using eFood.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eFood.Persistence.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly eFoodContext _db;

        public GenericRepository(eFoodContext db)
        {
            _db = db;
        }
         
        public async Task<AppsResult<T>> AddItem<T>(T entity, DbTransaction transaction = null) where T : class
        {
            var AppsResult = new AppsResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().Add(entity);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsResult.Entity = null;
                }

                AppsResult.Success = true;
                AppsResult.Entity = entity;
            }
            catch (Exception ex)
            {
                AppsResult.Success=false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsResult);
        }

        public async Task<AppsListResult<T>> AddRangeItems<T>(IEnumerable<T> entities, 
                                                            DbTransaction transaction = null) where T : class
        {
            var AppsListResult = new AppsListResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().AddRange(entities);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsListResult.Entities = null;
                }

                AppsListResult.Success = true;
                AppsListResult.Entities = entities;
            }
            catch (Exception ex)
            {
                AppsListResult.Success = false;
                AppsListResult.Entities = null;

                AppsListResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsListResult);
        }

        public async Task<AppsResult<T>> DeleteItem<T>(T entity, DbTransaction transaction = null) where T : class
        {
            var AppsResult = new AppsResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().Remove(entity);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsResult.Entity = null;
                }

                AppsResult.Success = true;
                AppsResult.Entity = entity;
            }
            catch (Exception ex)
            {
                AppsResult.Success = false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsResult);
        }

        public async Task<AppsListResult<T>> DeleteRangeItems<T>(IEnumerable<T> entities, DbTransaction transaction = null) where T : class
        {
            var AppsListResult = new AppsListResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().RemoveRange(entities);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsListResult.Entities = null;
                }

                AppsListResult.Success = true;
                AppsListResult.Entities = entities;
            }
            catch (Exception ex)
            {
                AppsListResult.Success = false;
                AppsListResult.Entities = null;

                AppsListResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsListResult);
        }

        public async Task<AppsListResult<T>> GetAll<T>() where T : class
        {
            var AppsListResult = new AppsListResult<T>();
            var queryable = _db.Set<T>().AsQueryable();

            try
            {
                AppsListResult.Entities = await queryable.ToListAsync();

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

        public async Task<AppsListResult<T>> GetAll<T,U>() where T : class where U : class
        {
            var AppsListResult = new AppsListResult<T>();
            var queryableInner = _db.Set<T>().AsQueryable();
            var queyableOther = _db.Set<U>().AsQueryable();

            //queryableInner = queryableInner.LeftJoin(queyableOther, c => c., o=> o.)


            return await Task.FromResult<AppsListResult<T>>(AppsListResult);
        }

        public async Task<AppsListResult<T>> GetAll<T>(
                                            Pagination pagination, 
                                            Expression<Func<T, bool>> search, 
                                            Func<IQueryable<T>, IOrderedQueryable<T>> order, 
                                            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) where T : class
        {
            var AppsListResult = new AppsListResult<T>();
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

        public string GetDBConnection()
        {
            return _db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<AppsResult<T>> GetOne<T>(int id) where T : class
        {
            var AppsResult = new AppsResult<T>();

            var result = await _db.Set<T>().FindAsync(id);

            if (Object.Equals(result,null))
            {
                AppsResult.Success = false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = "Entity not found" } };
            }

            AppsResult.Success = true;
            AppsResult.Entity = result;

            return await Task.FromResult<AppsResult<T>>(AppsResult);    
        }

        public async Task<AppsResult<T>> GetOne<T>(Expression<Func<T, bool>> search) where T : class
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

        public async Task<AppsResult> SaveChanges(DbTransaction transaction = null)
        {
            var AppsResult = new AppsResult();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = "Something passed!!" } };
                }

                AppsResult.Success = true;
            }
            catch (Exception ex)
            {
                AppsResult.Success = false;

                AppsResult.Errors.ToList().Add( new AppsError() { Code = "", ErrorDescription = ex.Message } );
            }

            return await Task.FromResult(AppsResult);
        }

        public async Task<AppsResult<T>> UpdateItem<T>(T entity, DbTransaction transaction = null) where T : class
        {
            var AppsResult = new AppsResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().Update(entity);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsResult.Entity = null;
                }

                AppsResult.Success = true;
                AppsResult.Entity = entity;
            }
            catch (Exception ex)
            {
                AppsResult.Success = false;
                AppsResult.Entity = null;

                AppsResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsResult);
        }

        public async Task<AppsListResult<T>> UpdateRangeItems<T>(IEnumerable<T> entities, DbTransaction transaction = null) where T : class
        {
            var AppsListResult = new AppsListResult<T>();

            try
            {
                if (!Object.Equals(transaction, null))
                {
                    _db.Database.UseTransaction(transaction);
                }

                _db.Set<T>().UpdateRange(entities);

                if (await _db.SaveChangesAsync() < 0)
                {
                    AppsListResult.Entities = null;
                }

                AppsListResult.Success = true;
                AppsListResult.Entities = entities;
            }
            catch (Exception ex)
            {
                AppsListResult.Success = false;
                AppsListResult.Entities = null;

                AppsListResult.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = ex.Message } };
            }

            return await Task.FromResult(AppsListResult);
        }
    }
}
