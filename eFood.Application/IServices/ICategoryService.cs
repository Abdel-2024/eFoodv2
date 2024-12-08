using eFood.Application.DTOs;
using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.IServices
{
    public interface ICategoryService
    {
        Task<AppsListResult<CategoryDTO>> GetAllCategory();
        Task<AppsListResult<CategoryDTO>> GetAllFilterCategory(CategoryFilterDTO filter);

        Task<AppsResult<CategoryDTO>> AddCategory(CategoryCreateDTO category);
        Task<AppsResult<CategoryDTO>> UpdateCategory(CategoryCreateDTO category, int Id);
        Task<AppsResult<CategoryDTO>> DeleteCategory(int Id);

        CategorySummaryDTO GetParentCategory(int Id);

        void AddPlusCategory();
    }
}
