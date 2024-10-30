using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReciclagemApi.Models;
using ReciclagemApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialModel>>> GetAllMaterials([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var materials = await _materialService.GetAllMaterialsAsync(page, pageSize);
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialModel>> GetMaterialById(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null) return NotFound();
            return Ok(material);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMaterial([FromBody] MaterialModel material)
        {
            await _materialService.AddMaterialAsync(material);
            return CreatedAtAction(nameof(GetMaterialById), new { id = material.MaterialId }, material);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] MaterialModel material)
        {
            material.MaterialId = id;
            await _materialService.UpdateMaterialAsync(material);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            await _materialService.DeleteMaterialAsync(id);
            return NoContent();
        }
    }
}
