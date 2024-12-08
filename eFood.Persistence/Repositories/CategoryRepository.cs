using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly eFoodContext _db;
        private readonly IDbContextTransaction _transaction;

        public CategoryRepository(eFoodContext db)
        {
            _db = db;
            _transaction = _db.Database.BeginTransaction();
        }

        public Category GetParentCategory(int Id)
        {
            return _db.Category.Where(c => c.CategoryId == Id).FirstOrDefault();
        }

        public Category AddPlusCategory()   
        {
            using (_transaction)
            {
                try
                {
                    var typeOption = new TypeOption();
                    typeOption.Name = "1st Grade";

                    var standard = _db.TypeOption.Add(typeOption);
                    _db.SaveChanges();

                    _db.Options.Add(new Options() { Name = "Computer Science 1", TypeOptionId= typeOption.TypeOptionId });
                    _db.Options.Add(new Options() { Name = "Computer Science 2", TypeOptionId = typeOption.TypeOptionId });
                    _db.SaveChanges();

                    _transaction.Commit();
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                }
            }
            var c = new Category();

            return c;
        }
    }
}
