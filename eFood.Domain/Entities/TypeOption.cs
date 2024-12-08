using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Domain.Entities
{
    public class TypeOption
    {
        public TypeOption()
        {
            Options = new HashSet<Options>();
        }

        public int TypeOptionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Options> Options { get; set; }
    }
}
