using AutoMapper;
using eFood.Application.DTOs;
using eFood.Application.IServices;
using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Infra.Storage.IServices;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eFood.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository _repo;
        private readonly IUnitOfWork _unitOfwork;   
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storage;
        private SqlConnection _connection; 
        private string containerName = "Products";

        public ProductsService( IGenericRepository genericRepository,    
                                IUnitOfWork unitOfwork,
                                IWebHostEnvironment env,
                                IMapper mapper,
                                IFileStorageService storage)
        {
            _repo = genericRepository;
            _unitOfwork = unitOfwork;
            _env = env;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<AppsResult<ProductsDTO>> AddProduct(ProductsCreateDTO model)
        {
            var result = new AppsResult<Products>();

            var product = _mapper.Map<Products>(model);

            if (!object.Equals(model.File, null))
            {
                product.ImgUrl = await _storage.SaveFile(containerName, model.File);
            }

            using (var _transaction = _unitOfwork.BeginTransaction())
            {
                try
                {
                    AppsResult<Products> resultProduct;
                    AppsListResult<ProductsOptions> resultPrdOpt;   

                    _unitOfwork.Add<Products>(product);
                    _unitOfwork.SaveChanges<Products>(product, out resultProduct);

                    if (!resultProduct.Success)
                            throw new InvalidOperationException(resultProduct.Errors.FirstOrDefault().ErrorDescription);
                    

                    if (!string.IsNullOrEmpty(model.Options))
                    {
                        var productsOptions = ConvertIDsToProductsOptions(model.Options, resultProduct.Entity.ProductId);

                        _unitOfwork.AddRange<ProductsOptions>(productsOptions);
                        _unitOfwork.SaveChanges<ProductsOptions>(productsOptions, out resultPrdOpt);    

                        if (!resultPrdOpt.Success)                      
                            throw new InvalidOperationException(resultProduct.Errors.FirstOrDefault().ErrorDescription);
                        
                    }

                    _transaction.Commit();
                    
                    result.Success = true;
                    result.Entity = resultProduct.Entity;
                }
                catch (Exception ex)
                {

                    _transaction.Rollback();

                    result.Success = false;
                    result.Errors.Add( new AppsError { Code = "", ErrorDescription = ex.Message });
                }           
            }

            var resultToReturn = _mapper.Map<AppsResult<ProductsDTO>>(result);

            if (!_env.IsDevelopment())
            {
                resultToReturn.Entity = null;
            }

            return resultToReturn;
        }

        public async Task<AppsResult<ProductsDTO>> UpdateProduct(ProductsCreateDTO model, int Id)
        {
            var result = new AppsResult<Products>();
            var product = new Products();

            _unitOfwork.GetOne<Products>(Id, out product);

            if (object.Equals(product, null))
            {
                result.Entity = null;
                result.Success = false;
                result.Errors = new List<AppsError>() { new AppsError() { Code = "", ErrorDescription = "Not found" } };

                return _mapper.Map<AppsResult<ProductsDTO>>(result);
            }

            var updatedProduct = _mapper.Map(model, product);

            if (!object.Equals(model.File, null))
            {
                updatedProduct.ImgUrl = await _storage.EditFile(containerName, model.File, updatedProduct.ImgUrl);
            }

            using (var _transaction = _unitOfwork.BeginTransaction())
            {
                try
                {
                    AppsResult<Products> resultProduct;
                    AppsListResult<ProductsOptions> resultPrdOpt;

                    _unitOfwork.SaveChanges<Products>(updatedProduct, out resultProduct);

                    if (!resultProduct.Success)                  
                        throw new InvalidOperationException(resultProduct.Errors.FirstOrDefault().ErrorDescription);
                    
                    IEnumerable<ProductsOptions> productsOptionsOld;

                    _unitOfwork.GetAll<ProductsOptions>(new Pagination(), c => c.ProductId == updatedProduct.ProductId, null, null, out productsOptionsOld);
                    _unitOfwork.RemoveRange<ProductsOptions>(productsOptionsOld);
                    _unitOfwork.SaveChanges<ProductsOptions>(productsOptionsOld, out resultPrdOpt);

                    if (!resultPrdOpt.Success)
                        throw new InvalidOperationException(resultProduct.Errors.FirstOrDefault().ErrorDescription);
                    

                    if (!string.IsNullOrEmpty(model.Options))
                    {
                        var productsOptionsNew = ConvertIDsToProductsOptions(model.Options, resultProduct.Entity.ProductId);

                        _unitOfwork.AddRange<ProductsOptions>(productsOptionsNew);
                        _unitOfwork.SaveChanges<ProductsOptions>(productsOptionsNew, out resultPrdOpt);

                        if (!resultPrdOpt.Success)                       
                            throw new InvalidOperationException(resultProduct.Errors.FirstOrDefault().ErrorDescription);
                        
                    }

                    _transaction.Commit();

                    result.Success = true;
                    result.Entity = resultProduct.Entity;
                }
                catch (Exception ex)
                {

                    _transaction.Rollback();

                    result.Success = false;
                    result.Errors.Add(new AppsError { Code = "", ErrorDescription = ex.Message });
                }
            }

            var resultToReturn = _mapper.Map<AppsResult<ProductsDTO>>(result);

            return resultToReturn;
        }

        private List<ProductsOptions> ConvertIDsToProductsOptions(string listIDs, int productId)        
        {
            var productsOptionsLst = new List<ProductsOptions>();

            if (String.IsNullOrEmpty(listIDs))
            {
                return productsOptionsLst;
            }

            if (listIDs.IndexOf(',') == -1 )
            {
                int optionId;
                if (Int32.TryParse(listIDs, out optionId))
                {
                    var productOption = new ProductsOptions();

                    productOption.ProductId = productId;
                    productOption.OptionId = optionId;

                    productsOptionsLst.Add(productOption);
                }
                else
                {
                    return productsOptionsLst;
                }
            }
            else
            {
                string[] splitString = listIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in splitString)
                {
                    var productOption = new ProductsOptions();

                    productOption.ProductId = productId;
                    productOption.OptionId = Convert.ToInt32(item);

                    productsOptionsLst.Add(productOption);
                }
            }

            return productsOptionsLst;
        }

        public async Task<AppsResult<ProductsDTO>> DeleteProduct(int Id)
        {
            var result = await _repo.GetOne<Products>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<ProductsDTO>>(result);
            }

            var deletingItem = await _repo.DeleteItem<Products>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<ProductsDTO>>(deletingItem);

            return resultToReturn;
        }

        public async Task<AppsListResult<ProductsDTO>> GetAllFilterProducts(ProductsFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<Products>();

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

                predicate = predicate.And(c => listIDs.All(i => i != c.ProductId));
            }

            if (!predicate.IsStarted)
            {
                predicate = null;
            }

            var result = await _repo.GetAll<Products>(filter.Pagination,
                                                                predicate,
                                                                c => c.OrderByDescending(n => n.ProductId),
                                                                c => c.Include(x => x.Category)
                                                                      .ThenInclude(p => p.Parent)
                                                                      .Include(n => n.ProductsOptions)
                                                                      .ThenInclude( o => o.Options)
                                                                      .ThenInclude( t => t.TypeOption));

            var resultToReturn = _mapper.Map<AppsListResult<ProductsDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsListResult<ProductsDTO>> GetAllProducts()
        {
            var result = await _repo.GetAll<Products>();

            var resultToReturn = _mapper.Map<AppsListResult<ProductsDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsResult<ProductsDTO>> GetOneProduct(int Id)
        {
            var result = await _repo.GetOne<Products>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<ProductsDTO>>(result);
            }

            var deletingItem = await _repo.DeleteItem<Products>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<ProductsDTO>>(deletingItem);

            return resultToReturn;
        }

        public Task<AppsListResult<ProductsDTO>> GetSomeProducts()
        {
            throw new NotImplementedException();
        }
    }
}
