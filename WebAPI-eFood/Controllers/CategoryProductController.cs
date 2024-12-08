using eFood.Application.IServices;
using eFood.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
        private readonly ICategoryProductService _categoryProductService;
        private readonly IJsonSerializer _json;

        public CategoryProductController(ICategoryProductService categoryProductService, 
                                         IJsonSerializer json)
        {
            _categoryProductService = categoryProductService;
            _json = json;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result= await _categoryProductService.GetAllCategoryProduct();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpGet("allfilter")]
        public async Task<IActionResult> GetAll([FromQuery]CategoryProductFilterDTO model)
        {
            var result = await _categoryProductService.GetAllFilterCategoryProduct(model);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]CategoryProductCreateDTO model)
        {
            if (object.Equals(model,null))
            {
                return BadRequest("Model not valid");
            }

            var result = await _categoryProductService.AddCategoryProduct(model);

            if (result.Success==false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] CategoryProductCreateDTO model, int id)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("Model not valid");
            }

            var result =await _categoryProductService.UpdateCategoryProduct(model, id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }
    }
}
