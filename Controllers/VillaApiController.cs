using Learning.CoreApi.Data;
using Learning.CoreApi.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Learning.CoreApi.Controllers {
    [Route("api/[controller]")]
    //[ApiController]
    public class VillaApiController : ControllerBase {
        private readonly ILogger<VillaApiController> _logger;
        public VillaApiController(ILogger<VillaApiController> _logger) {
            this._logger = _logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas() {
            this._logger.LogInformation("Get all Villas");
            return Ok(VillaStore.VillaList);
        }
        [HttpGet("{Id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO?> GetVilla(int Id) {
            if (Id <= 0) {
                this._logger.LogError($"GetVilla Error with id = {Id}");
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(villa => villa.Id == Id);
            if (villa == null) {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDto) {
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}

            if (VillaStore.VillaList.FirstOrDefault(villa => villa.Name.ToLower() == villaDto.Name.ToLower()) != null) {
                ModelState.AddModelError("VillaUnique", "villa already exists!");
                return BadRequest(ModelState);
            }
            if (villaDto == null) {
                return BadRequest();
            }
            if (villaDto.Id > 0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.VillaList.OrderByDescending(villa => villa.Id)
                                              .FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDto);
            return CreatedAtRoute("GetVilla", new { Id = villaDto.Id }, villaDto);
        }
        [HttpDelete("{Id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int Id) {
            if (Id == 0) {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == Id);
            if (villa == null) {
                return NotFound();
            }
            VillaStore.VillaList.Remove(villa);
            return NoContent();
        }
        [HttpPut("{Id:int}", Name = "UpdateVilla")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int Id, [FromBody] VillaDTO villaDto) {
            if (villaDto == null || Id != villaDto.Id) {
                return BadRequest();
            }
            // retrieve the villa 
            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == Id);
            if (villa == null) {
                return NotFound();
            }
            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.Sqft = villaDto.Sqft;

            return NoContent();
        }

        [HttpPatch("{Id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int Id, [FromBody] JsonPatchDocument<VillaDTO> patchDto) {
            if (patchDto == null || Id == 0) {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == Id);
            if (villa == null) {
                return NotFound();
            }
            // apply changes to villa instance 
            patchDto.ApplyTo(villa, ModelState);
            // if ModelState is not valid 
            // return badRequest with errors
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
