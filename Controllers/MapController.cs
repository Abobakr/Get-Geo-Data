using adsmap.Models;
using System.Linq;
using System.Security.Claims;
using adsmap.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace adsmap.Controllers
{
    public class MapController : Controller
    {
        private readonly GlobalTypeRepository globalTypeRepository = new GlobalTypeRepository();

        // GET: Map
        public IActionResult Index()
        {

            User user = new User();

            var userIdValue = "1";

            //var claimsIdentity = User.Identity as ClaimsIdentity;
            //if (claimsIdentity != null)
            //{
            //    // the principal identity is a claims identity.
            //    // now we need to find the NameIdentifier claim
            //    var userIdClaim = claimsIdentity.Claims
            //        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            //    if (userIdClaim != null)
            //    {
            //        userIdValue = userIdClaim.Value;
            //    }
            //}

            //map.UserId = userIdValue;

            user.UserId = userIdValue;

            user.Center = globalTypeRepository.GetUserMapCenter(userIdValue);

            user.FilterComponents = globalTypeRepository.GetFilterComponents(userIdValue);

            return View(user);
        }
    }
}