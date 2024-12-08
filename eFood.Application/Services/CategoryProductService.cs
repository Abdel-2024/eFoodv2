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
using System.Linq;
using System.Threading.Tasks;

namespace eFood.Application.Services
{
    public class CategoryProductService : ICategoryProductService
    {
        private readonly IGenericRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storage;
        private string containerName = "CategoryProduct";

        public CategoryProductService(IGenericRepository genericRepository,
                                        IWebHostEnvironment env,
                                        IMapper mapper, 
                                        IFileStorageService storage)
        {
            _repo = genericRepository;
            _env = env;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<AppsResult<CategoryProductDTO>> AddCategoryProduct(CategoryProductCreateDTO model)
        {
            var categoryProduct = _mapper.Map<CategoryProduct>(model);

            if (!object.Equals(model.File, null))
            {
                categoryProduct.ImgUrl = await _storage.SaveFile(containerName, model.File);
            }

            var result = await _repo.AddItem<CategoryProduct>(categoryProduct);

            var resultToReturn = _mapper.Map<AppsResult<CategoryProductDTO>>(result);

            if (!_env.IsDevelopment())
            {
                resultToReturn.Entity = null;
            }

            return resultToReturn;
        }

        public async Task<AppsResult<CategoryProductDTO>> DeleteCategoryProduct(int id)
        {
            var result = await _repo.GetOne<CategoryProduct>(id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<CategoryProductDTO>>(result);
            }

            var categoryProductDeleted = _repo.DeleteItem<CategoryProduct>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<CategoryProductDTO>>(categoryProductDeleted);

            return resultToReturn;
        }

        public async Task<AppsListResult<CategoryProductDTO>> GetAllCategoryProduct()
        {
           var result=await _repo.GetAll<CategoryProduct>();

           var resultToReturn = _mapper.Map<AppsListResult<CategoryProductDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsListResult<CategoryProductDTO>> GetAllFilterCategoryProduct(CategoryProductFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<CategoryProduct>();

            if (!string.IsNullOrEmpty(filter.CategoryProductName))
            {
                predicate=predicate.And(c=>c.CategoryProductName.Contains(filter.CategoryProductName));
            }

            if (!predicate.IsStarted)
            {
                predicate = null;
            }

            var result = await _repo.GetAll<CategoryProduct>(filter.Pagination,
                                                                predicate,
                                                                c=>c.OrderByDescending(n=>n.CategoryProductId),
                                                                c=>c.Include(n=>n.Products));

            var resultToReturn = _mapper.Map<AppsListResult<CategoryProductDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsResult<CategoryProductDTO>> UpdateCategoryProduct(CategoryProductCreateDTO model, int id)
        {
            var result =await _repo.GetOne<CategoryProduct>(id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<CategoryProductDTO>>(result);
            }

            var categoryProduct= _mapper.Map<CategoryProduct>(model);

            if (!object.Equals(model.File, null))
            {
                categoryProduct.ImgUrl = await _storage.EditFile(containerName, model.File, categoryProduct.ImgUrl);
            }
            categoryProduct.CategoryProductId = id;

            var categoryProductUpdated= _repo.UpdateItem<CategoryProduct>(categoryProduct);

            var resultToReturn = _mapper.Map<AppsResult<CategoryProductDTO>>(categoryProductUpdated);

            return resultToReturn;
        }
    }
}
