using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class CategoryProduct
    {
        public CategoryProduct()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryProductId { get; set; }
        public string CategoryProductName { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
