using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Model;

namespace PoseidonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet, Route("Create")]
        public async Task<IActionResult> Create(string login, string password)
        {
            var model = new IdentityUser { UserName = login };
            var result = await _userManager.CreateAsync(model, password);
            return Ok(result);
        }

        [HttpGet, Route("Login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var model = await _userManager.FindByNameAsync(login);
            if(model != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model, password, false, false);
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet, Route("logout")]
        public async Task<IActionResult> logout()
        {
            if(_signInManager.IsSignedIn(User)) await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("GetMe")]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await _userManager.GetUserAsync(User));
        }
    }
}
