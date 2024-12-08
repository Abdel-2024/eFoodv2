using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.IServices
{
    public interface IJsonSerializer
    {
        string SerializeObject(object obj);
    }
}
