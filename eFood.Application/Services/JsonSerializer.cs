using eFood.Application.IServices;
using Newtonsoft.Json;

namespace eFood.Application.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        public string SerializeObject(object obj)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //};

            //return JsonConvert.SerializeObject(object, settings);

            return JsonConvert.SerializeObject(obj);
        }
    }
}
