using eFood.Application.DTOs;
using eFood.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IJsonSerializer _json;

        public CategoryController(ICategoryService categoryService,
                                  IJsonSerializer json)
        {
            _categoryService = categoryService;
            _json = json;
        }

        [HttpGet("test")]
        public  IActionResult Test()
        {
            _categoryService.AddPlusCategory();

            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategory();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = result.Entities
                    .Where(c => c.ParentId == null)
                    .Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Description = c.Description,
                        ImgUrl = c.ImgUrl,
                        ParentId = c.ParentId,
                        Parent = null,
                        Children = GetChildren(result.Entities, c.CategoryId)
                    })
                    .ToList();

            var resultToReturnJson = _json.SerializeObject(resultToReturn);

            return Ok(resultToReturnJson);
        }

        [HttpGet("allfilter")]
        public async Task<IActionResult> GetAll([FromQuery] CategoryFilterDTO model)
        {
            var result = await _categoryService.GetAllFilterCategory(model);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = result.Entities
                    .Where(c => c.ParentId == null)
                    .Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Description = c.Description,
                        ImgUrl = c.ImgUrl,
                        ParentId = c.ParentId,
                        Parent =  GetParent(c.CategoryId),
                        Children =  GetChildren(result.Entities, c.CategoryId)
                    })
                    .ToList();

            var resultToReturnJson = _json.SerializeObject(resultToReturn);

            return Ok(resultToReturnJson);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] CategoryCreateDTO model)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("Model not valid");
            }

            var result = await _categoryService.AddCategory(model);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] CategoryCreateDTO model, int id)
        {
            if (object.Equals(model, null))
            {
                return BadRequest("data not valid");
            }

            var result = await _categoryService.UpdateCategory(model, id);

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

            var result = await _categoryService.DeleteCategory(id);

            if (result.Success == false)
            {
                return BadRequest(result.Errors);
            }

            var resultToReturn = _json.SerializeObject(result.Entity);

            return Ok(resultToReturn);
        }

        private List<CategoryDTO> GetChildren(IEnumerable<CategoryDTO> categories, int parentId)   
        {
            return categories
                    .Where(c => c.ParentId == parentId)
                    .Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Description = c.Description,
                        ImgUrl = c.ImgUrl,
                        ParentId = c.ParentId,
                        Parent = GetParent(parentId),
                        Children = GetChildren(categories, c.CategoryId)
                    })
                    .ToList();
        }

        private CategorySummaryDTO GetParent(int parentId)   
        {
            return _categoryService.GetParentCategory(parentId);
        }

    }
}
