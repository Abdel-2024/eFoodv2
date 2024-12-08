using eFood.Application.DTOs;
using eFood.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _optionsService;
        private readonly IJsonSerializer _json;

        public OptionsController(IOptionsService optionsService,
                                  IJsonSerializer json)
        {
            _optionsService = optionsService;
            _json = json;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _optionsService.GetAllOptions();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpGet("allfilter")]
        public async Task<IActionResult> GetAll([FromQuery] OptionsFilterDTO model)
        {
            var result = await _optionsService.GetAllFilterOptions(model);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entities);

            return Ok(resultToReturn);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] OptionsCreateDTO model)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("Model not valid");
            }

            var result = await _optionsService.AddOption(model);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] OptionsCreateDTO model, int id)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("data not valid");
            }

            var result = await _optionsService.UpdateOption(model, id);

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

            var result = await _optionsService.DeleteOption(id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }
    }
}
