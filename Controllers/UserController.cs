using adsmap.DataAccessLayer;
using adsmap.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace adsmap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GlobalTypeRepository globalTypeRepository = new GlobalTypeRepository();
        
        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMapCenter(string id, LocationType curr)
        {
            bool succsessful = await globalTypeRepository.UpdateCenter(id, curr.Lat, curr.Lng);

            if (!succsessful)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationType>> GetUserMapCenter(string id)
        {
            var userMapCenter = await globalTypeRepository.GetUserMapCenterAsync(id);

            if (userMapCenter == null)
            {
                return NotFound();
            }

            return userMapCenter;
        }
    }


}
