using AutoMapper;
using eFood.Application.DTOs;
using eFood.Application.IServices;
using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Infra.Storage.IServices;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository _repo;
        private readonly IUnitOfWork _unitOfWork;   
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storage;
        private string containerName = "Category";

        public CategoryService( IGenericRepository genericRepository,
                                IUnitOfWork unitOfWork,
                                IWebHostEnvironment env,
                                IMapper mapper,
                                IFileStorageService storage)
        {
            _repo = genericRepository;
            _env = env;
            _mapper = mapper;
            _storage = storage;
            _unitOfWork = unitOfWork;
        }

        public void AddPlusCategory()
        {
            //using (var transaction = _unitOfWork.BeginTransaction())
            //{
            //    try
            //    {
            //        var typeOption = new TypeOption();
            //        typeOption.Name = "1st Grade";

            //        _unitOfWork.Add<TypeOption>(typeOption);
            //        var result=_unitOfWork.SaveChanges(typeOption); 

            //        _unitOfWork.Add<Options>(new Options() { Name = "Computer Science 1", TypeOptionId = result.Entity.TypeOptionId });
            //        _unitOfWork.Add<Options>(new Options() { Name = "Computer Science 2", TypeOptionId = result.Entity.TypeOptionId });
            //        _unitOfWork.SaveChanges();

            //        transaction.Commit();
            //    }
            //    catch (Exception)
            //    {
            //        transaction.Rollback();
            //    }
            //}
        }

        public async Task<AppsResult<CategoryDTO>> AddCategory(CategoryCreateDTO model)
        {
            var category = _mapper.Map<Category>(model);

            if (!object.Equals(model.File, null))
            {
                category.ImgUrl = await _storage.SaveFile(containerName, model.File);
            }

            var result = await _repo.AddItem<Category>(category);

            var resultToReturn = _mapper.Map<AppsResult<CategoryDTO>>(result);

            if (!_env.IsDevelopment())
            {
                resultToReturn.Entity = null;
            }

            return resultToReturn;
        }

        public async Task<AppsResult<CategoryDTO>> DeleteCategory(int Id)
        {
            var result = await _repo.GetOne<Category>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<CategoryDTO>>(result);
            }

            var deletingResult =await _repo.DeleteItem<Category>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<CategoryDTO>>(deletingResult);

            return resultToReturn;
        }

        public async Task<AppsListResult<CategoryDTO>> GetAllCategory()
        {            var result = await _repo.GetAll<Category>();

            var resultToReturn = _mapper.Map<AppsListResult<CategoryDTO>>(result);

            return resultToReturn;

        }

        public async Task<AppsListResult<CategoryDTO>> GetAllFilterCategory(CategoryFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<Category>();

            if (filter.Page != 0)
            {
                filter.Pagination.Page = filter.Page;
            }

            if (filter.PageSize != 0)
            {
                filter.Pagination.PageSize = filter.PageSize;
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate = predicate.And(c => c.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.ListIDs))
            {
                string[] splitString = filter.ListIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> listIDs = new List<int>();

                foreach (string item in splitString)
                {
                    listIDs.Add(Convert.ToInt32(item));
                }

                predicate = predicate.And(c=> listIDs.All(i => i != c.CategoryId));
            }

            if (!predicate.IsStarted)
            {
                predicate = null;
            }

            var result = await _repo.GetAll<Category>(filter.Pagination,
                                                        predicate,
                                                        c => c.OrderByDescending(n => n.CategoryId),
                                                        c => c.Include(n => n.Products));

            var resultToReturn = _mapper.Map<AppsListResult<CategoryDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsResult<CategoryDTO>> UpdateCategory(CategoryCreateDTO model, int Id)
        {
            var result = await _repo.GetOne<Category>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<CategoryDTO>>(result);
            }

            var category = _mapper.Map(model, result.Entity);

            if (!object.Equals(model.File, null))
            {
                category.ImgUrl = await _storage.EditFile(containerName, model.File, category.ImgUrl);
            }
 
            var savingResult = await _repo.SaveChanges();

            if (savingResult.Success)
            {
                result.Entity = category;
            }

            result.Success = savingResult.Success;       
            result.Errors = savingResult.Errors;

            var resultToReturn = _mapper.Map<AppsResult<CategoryDTO>>(result);

            return resultToReturn;
        }

        CategorySummaryDTO ICategoryService.GetParentCategory(int Id)
        {
            var result = new Category();

            _unitOfWork.GetOne<Category>(c => c.CategoryId == Id, out result);

            var resultToReturn = _mapper.Map<CategorySummaryDTO>(result);

            return resultToReturn;

        }
    }
}
