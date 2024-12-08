using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class ProductsFilterDTO
    {
        public ProductsFilterDTO()
        {
            Pagination = new Pagination();
        }

        public string Name { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public Pagination Pagination { get; set; }

        public string ListIDs { get; set; }
    }
}
