using eFood.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class CategoryProductDTO
    {
        public CategoryProductDTO()
        {
            ProductsDTO = new HashSet<ProductsDTO>();
        }

        public int CategoryProductId { get; set; }
        public string CategoryProductName { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<ProductsDTO> ProductsDTO { get; set; }
    }
}
