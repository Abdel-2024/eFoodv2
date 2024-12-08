
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eFood.Application.DTOs
{
    public class CategoryProductCreateDTO
    {
        public string CategoryProductName { get; set; }

        public string Description { get; set; }

        public IFormFile File { get; set; }
    }
}
