using eFood.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class CategoryProductFilterDTO
    {
        public CategoryProductFilterDTO()
        {
            Pagination = new Pagination();
            Pagination.Page = 1;
            Pagination.PageSize = 5;

            ListIDs = new List<int>();
        }

        public string CategoryProductName { get; set; }
 
        public Pagination Pagination { get; set; }
        public IEnumerable<int> ListIDs { get; set; }
    }
}
