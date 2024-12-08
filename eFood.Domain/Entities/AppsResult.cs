using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class AppsResult<T> where T : class 
    {
        public AppsResult()
        {
            Errors = new List<AppsError>();
        }
        public bool Success { get; set; }
        public T Entity { get; set; }
        public List<AppsError> Errors { get; set; }
    }

    public class AppsResult
    {
        public bool Success { get; set; }
        public List<AppsError> Errors { get; set; }
    }
}
