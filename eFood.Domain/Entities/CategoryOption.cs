using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class CategoryOption
    {
        public CategoryOption()
        {
            Options = new HashSet<Options>();
        }

        public int CategoryOptionId { get; set; }
        public string CategoryOptionName { get; set; }  
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Options> Options { get; set; }
    }
}
