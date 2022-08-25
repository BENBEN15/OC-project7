using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Contracts.User;
using PoseidonAPI.Validators;

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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteMyUser(DeleteMyselfRequest request)
        {
            var user = _userManager.FindByNameAsync(request.login);
            var confirmed = await _userManager.CheckPasswordAsync(await user, request.Password);
            if (confirmed)
            {
                try
                {
                    var userToDelete = await user;
                    await _userManager.DeleteAsync(userToDelete);
                    return Ok("userDeleted");
                } 
                catch
                {
                    return BadRequest();
                }
            } else
            {
                return BadRequest("Wrong password or user name");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateMyUser(UpdateUserRequest request)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser.UserName == request.userName)
            {
                UserValidator userValidator = new UserValidator();
                var ValidatorResult = userValidator.Validate(request);
                if(ValidatorResult.IsValid)
                {
                    var userToUpdate = await _userManager.FindByNameAsync(request.userName);
                    userToUpdate.UserName = request.newUserName;
                    userToUpdate.PhoneNumber = request.phoneNumber;
                    userToUpdate.Email = request.email;

                    try
                    {
                        await _userManager.UpdateAsync(userToUpdate);
                        return Ok("success");
                    }
                    catch
                    {
                        return BadRequest("error");
                    }
                }
                else
                {
                    List<ErrorModel> errors = new List<ErrorModel>();
                    foreach (var failure in ValidatorResult.Errors)
                    {
                        ErrorModel error = new ErrorModel
                        {
                            errorCode = failure.ErrorCode,
                            errorField = failure.PropertyName,
                            errorMessage = failure.ErrorMessage,
                        };

                        errors.Add(error);
                    }

                    return BadRequest(errors);
                }
            } 
            else
            {
                return NotFound();
            }  
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
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("current")]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await _userManager.GetUserAsync(User));
        }
    }
}
