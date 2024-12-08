using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eFood.Application.DTOs
{
    public class ProductsCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; } 
        public decimal Price { get; set; }
        public bool IsNew { get; set; }
        public bool IsRecommended { get; set; }
        public int CategoryId { get; set; }

        public string Options { get; set; } 
    }
}
