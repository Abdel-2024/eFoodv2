using eFood.Application.DTOs;
using eFood.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IJsonSerializer _json;

        public ProductsController(IProductsService productsService,
                                  IJsonSerializer json)
        {
            _productsService = productsService;
            _json = json;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productsService.GetAllProducts();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpGet("allfilter")]
        public async Task<IActionResult> GetAll([FromQuery] ProductsFilterDTO model)
        {
            var result = await _productsService.GetAllFilterProducts(model);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] ProductsCreateDTO model)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("Model not valid");
            }

            var result = await _productsService.AddProduct(model);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] ProductsCreateDTO model, int id)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("data not valid");
            }

            var result = await _productsService.UpdateProduct(model, id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("data not valid");
            }

            var result = await _productsService.DeleteProduct(id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }
    }
}
