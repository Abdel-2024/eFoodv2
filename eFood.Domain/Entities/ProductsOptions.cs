using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class ProductsOptions
    {
        public int OptionId { get; set; }
        public int ProductId { get; set; } 

        public Options Options { get; set; }
        public Products Products { get; set; }    
    }
}
