using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class Products
    {
        public Products()
        {
            ProductsOptions = new HashSet<ProductsOptions>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsRecommended { get; set; } 

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductsOptions> ProductsOptions { get; set; }   
    }
}
