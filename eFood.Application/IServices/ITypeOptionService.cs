using eFood.Application.DTOs;
using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.IServices
{
    public interface ITypeOptionService
    {
        Task<AppsListResult<TypeOptionDTO>> GetAllTypeOption();
        Task<AppsListResult<TypeOptionDTO>> GetAllFiltredTypeOption(TypeOptionFilterDTO filter);

        Task<AppsResult<TypeOptionDTO>> AddTypeOption(TypeOptionCreateDTO category);
        Task<AppsResult<TypeOptionDTO>> UpdateTypeOption(TypeOptionCreateDTO category, int Id);
        Task<AppsResult<TypeOptionDTO>> DeleteTypeOption(int Id);
    }
}
