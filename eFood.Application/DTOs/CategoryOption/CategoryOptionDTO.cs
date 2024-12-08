using eFood.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class CategoryOptionDTO
    {
        public CategoryOptionDTO()
        {
            Options = new HashSet<OptionsDTO>();
        }

        public int CategoryOptionId { get; set; }
        public string CategoryOptionName { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<OptionsDTO> Options { get; set; }
    }
}
