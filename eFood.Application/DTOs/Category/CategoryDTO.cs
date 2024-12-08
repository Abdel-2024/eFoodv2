using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class CategoryDTO    
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public int? ParentId { get; set; }
        public CategorySummaryDTO Parent { get; set; }

        public ICollection<CategoryDTO> Children { get; set; }
       //public ICollection<ProductsDTO> Products { get; set; }  
    }
}
