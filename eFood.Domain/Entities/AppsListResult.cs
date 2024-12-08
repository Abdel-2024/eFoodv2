using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class AppsListResult<T> where T : class 
    {
        public bool Success { get; set; }
        public IEnumerable<T> Entities { get; set; }
        public IEnumerable<AppsError> Errors { get; set; }
        public int TotalRecords { get; set; } 
    }
}
