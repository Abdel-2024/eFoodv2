using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class CategoryCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int? ParentId { get; set; }
    }
}
