using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.IRepositories
{
    public interface ICategoryRepository
    {
        Category GetParentCategory(int Id);
        
        Category AddPlusCategory();
    }
}
