using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using System.Drawing.Text;

namespace OnlineShop.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpDelete]
        public async Task< IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user==null)
                 return NotFound();

          var result =  await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception();

            return Ok();
        }
    }
}
