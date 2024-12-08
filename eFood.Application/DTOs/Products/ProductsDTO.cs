
using eFood.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class ProductsDTO
    {
        public ProductsDTO()
        {
            Options = new HashSet<OptionsDTO>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsNew { get; set; }
        public bool IsRecommended { get; set; }

        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }   

        public ICollection<OptionsDTO> Options { get; set; }
    }
}
