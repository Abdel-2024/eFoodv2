using eFood.Application.DTOs;
using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.IServices
{
    public interface IOptionsService
    {
        Task<AppsListResult<OptionsDTO>> GetAllOptions();
        Task<AppsListResult<OptionsDTO>> GetAllFilterOptions(OptionsFilterDTO filter);

        Task<AppsResult<OptionsDTO>> AddOption(OptionsCreateDTO category);
        Task<AppsResult<OptionsDTO>> UpdateOption(OptionsCreateDTO category, int Id);
        Task<AppsResult<OptionsDTO>> DeleteOption(int Id);  
    }
}
