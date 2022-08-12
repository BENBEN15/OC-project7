using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.User;
using PoseidonAPI.Model;

namespace PoseidonAPI.Controllers
{
    [Route("api/users")]
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var model = new IdentityUser { 
                UserName = request.Username, 
                PhoneNumber = request.Phonenumber, 
                Email = request.Email  
            };
            var result = await _userManager.CreateAsync(model, request.Password);
            return Ok(result);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var model = await _userManager.FindByNameAsync(request.login);
            if(model != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model, request.Password, false, false);
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet, Route("logout")]
        public async Task<IActionResult> logout()
        {
            if(_signInManager.IsSignedIn(User)) await _signInManager.SignOutAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet, Route("current")]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await _userManager.GetUserAsync(User));
        }
    }
}
