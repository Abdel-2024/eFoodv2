using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class Options
    {
        public Options()
        {
            ProductsOptions = new HashSet<ProductsOptions>();
        }

        public int OptionId { get; set; }   
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public int TypeOptionId { get; set; }   
        public TypeOption TypeOption { get; set; }  

        public ICollection<ProductsOptions> ProductsOptions { get; set; }
    }
}
