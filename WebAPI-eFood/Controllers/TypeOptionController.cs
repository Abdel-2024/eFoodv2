using eFood.Application.DTOs;
using eFood.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOptionController : ControllerBase
    {
        private readonly ITypeOptionService _typeOptionService;
        private readonly IJsonSerializer _json;

        public TypeOptionController(ITypeOptionService typeOptionervice,
                                  IJsonSerializer json)
        {
            _typeOptionService = typeOptionervice;
            _json = json;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _typeOptionService.GetAllTypeOption();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpGet("allfilter")]
        public async Task<IActionResult> GetAll([FromQuery] TypeOptionFilterDTO model)
        {
            var result = await _typeOptionService.GetAllFiltredTypeOption(model);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] TypeOptionCreateDTO model)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("Model not valid");
            }

            var result = await _typeOptionService.AddTypeOption(model);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] TypeOptionCreateDTO model, int id)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("data not valid");
            }

            var result = await _typeOptionService.UpdateTypeOption(model, id);

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

            var result = await _typeOptionService.DeleteTypeOption(id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

    }
}
