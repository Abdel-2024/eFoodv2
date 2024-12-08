
using eFood.Domain.Entities;
using eFood.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace eFood.Application.IServices
{
    public interface ICategoryProductService
    {
        Task<AppsListResult<CategoryProductDTO>> GetAllCategoryProduct();
        Task<AppsListResult<CategoryProductDTO>> GetAllFilterCategoryProduct(CategoryProductFilterDTO filter);

        Task<AppsResult<CategoryProductDTO>> AddCategoryProduct(CategoryProductCreateDTO categoryProduct);
        Task<AppsResult<CategoryProductDTO>> UpdateCategoryProduct(CategoryProductCreateDTO categoryProduct, int Id);
        Task<AppsResult<CategoryProductDTO>> DeleteCategoryProduct(int Id);
    }
}
