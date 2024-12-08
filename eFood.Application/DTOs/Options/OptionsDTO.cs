
using eFood.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class OptionsDTO
    {
        public OptionsDTO()
        {
            Products = new HashSet<ProductsDTO>();
        }

        public int OptionId { get; set; }   
        public string Name { get; set; }    
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public int TypeOptionId { get; set; }
        public TypeOptionSummaryDTO TypeOption { get; set; }   

        public ICollection<ProductsDTO> Products { get; set; }
    }
}
