using eFood.Application.DTOs;
using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.IServices
{
    public interface IProductsService
    {
        Task<AppsListResult<ProductsDTO>> GetAllProducts();  
        Task<AppsListResult<ProductsDTO>> GetAllFilterProducts(ProductsFilterDTO filter);

        Task<AppsResult<ProductsDTO>> GetOneProduct(int Id);

        Task<AppsListResult<ProductsDTO>> GetSomeProducts();

        Task<AppsResult<ProductsDTO>> AddProduct(ProductsCreateDTO product);
        Task<AppsResult<ProductsDTO>> UpdateProduct(ProductsCreateDTO product, int Id); 
        Task<AppsResult<ProductsDTO>> DeleteProduct(int Id);    
    }
}
