using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoseidonAPI.Contracts.Error;
using PoseidonAPI.Contracts.Base;
using PoseidonAPI.Contracts.User;
using PoseidonAPI.Validators;
using PoseidonAPI.Services;
using AutoMapper;
using System.Net.Mime;

namespace PoseidonAPI.Controllers
{
    [Route("/users")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /users
        ///     
        /// </remarks>
        /// <response code="200">Returns all users</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            var users = new List<UserResponse>();
            foreach (var user in result)
            {
                users.Add(_mapper.Map<UserResponse>(user));
            }
            return Ok(users);
        }

        /// <summary>
        /// Return a user for a given userName
        /// </summary>
        /// <param name="userName">the username of the user you want to retrieve</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /users/username1
        ///     
        /// </remarks>
        /// <response code="200">Returns a single user corresponding to the username</response>
        /// <response code="400">The username sent does not exist</response>
        [HttpGet, Route("{userName}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result != null)
            {
                var user = _mapper.Map<UserResponse>(result);
                return Ok(user);
            } else
            {
                return BadRequest(new ResponseBase(false, 400, "username_not_found" ,"username not found"));
            }
        }

        /// <summary>
        /// Create a user and adds it to the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /users
        ///     {
        ///         "username": "testUser1",
        ///         "email": "test1@gmail.com",
        ///         "phoneNumber": "01 00 00 00 00",
        ///         "password": "@testUser1"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Creation succesfull</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ResponseBase>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var validator = new CreateUserValidator();
            var validatorResult = validator.Validate(request);
            if (validatorResult.IsValid)
            {
                var model = new IdentityUser { 
                    UserName = request.Username, 
                    PhoneNumber = request.Phonenumber, 
                    Email = request.Email  
                };
                var result = await _userManager.CreateAsync(model, request.Password);
                if (result != null)
                {
                    return Ok();
                } 
                else
                {
                    return BadRequest(new ResponseBase(false, 400, "creation_failed", "creation failed"));
                }
            }
            else
            {
                List<ErrorModel> errors = new List<ErrorModel>();
                foreach (var failure in validatorResult.Errors)
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

        /// <summary>
        /// Update a user
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     PUT /users
        ///     {
        ///         "userName": "testUser1",
        ///         "newUserName": "testUser2",
        ///         "email": "test2@gmail.com",
        ///         "phoneNumber": "02 00 00 00 00"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Update succesfull, the entity have been successfully updated</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to access this ressource</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                        return Ok();
                    }
                    catch
                    {
                        return BadRequest(new ResponseBase(false, 400, "update_failed","update failed"));
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
                return BadRequest(new ResponseBase(false, 400, "not_logged_in", "you should be logged in"));
            }  
        }

        /// <summary>
        /// Delete your account(logged in account)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     DELETE /users
        ///     
        /// </remarks>
        /// <response code="200">Deletion succesfull, the entity have been successfully deleted</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        /// <response code="404">You must be logged in to perform this action</response>
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                    return Ok();
                } 
                catch
                {
                    return BadRequest(new ResponseBase(false, 400, "delete_failed", "deletion failed"));
                }
            } 
            else
            {
                return BadRequest(new ResponseBase(false, 400, "invalid_credentials", "Wrong password or user name"));
            }
        }

        /// <summary>
        /// Log to your account using credentials
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /users/login
        ///     
        /// </remarks>
        /// <response code="200">Returns result of loggin action</response>
        /// <response code="400">Error in request, login or password incorrect</response>
        [HttpPost, Route("login")]
        [ProducesResponseType( typeof(Microsoft.AspNetCore.Identity.SignInResult),StatusCodes.Status200OK)]
        [ProducesResponseType( typeof(ResponseBase),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var model = await _userManager.FindByNameAsync(request.login);
            if(model != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model, request.Password, false, false);
                return Ok(result);
            }
            return BadRequest(new ResponseBase(false, 400, "invalid_credentials", "username or password incorrect"));
        }

        /// <summary>
        /// log out of your account
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /users/logout
        ///     
        /// </remarks>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Logout failed</response>
        [HttpGet, Route("logout")]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> logout()
        {
            if (_signInManager.IsSignedIn(User)) { 
                await _signInManager.SignOutAsync();
                return Ok(new ResponseBase(true, 200, "logout_success", "you have been logged out successfully"));
            } else {
                return BadRequest(new ResponseBase(false, 400, "logout_failed", "log out has failed"));
            }
            
        }

        /// <summary>
        /// get logged in user infos
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     GET /users/current
        ///     
        /// </remarks>
        /// <response code="200">current(logged in) user infos</response>
        /// <response code="400">Error, unable to retrieve current user</response>
        [Authorize]
        [HttpGet, Route("current")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                var result = _mapper.Map<UserResponse>(user);
                return Ok(result);
            } else
            {
                return BadRequest();
            }
            
        }

        /// <summary>
        /// Send forgot password request
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /users/forgotPassword
        ///     
        /// </remarks>
        /// <response code="200">send reset password email</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        [HttpPost, Route("forgotPassword")]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorModel>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseBase),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var validator = new ForgotPasswordValidator();
            var ValidatorResult = validator.Validate(request);
            if (ValidatorResult.IsValid)
            {
                try
                {
                var user = await _userManager.FindByEmailAsync(request.email);
                    if (user != null)
                    {
                        //TODO add mail sender services, token returned only for tests purposes 
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                        var response = await _emailService.sendResetPasswordEmail(token, request.email);

                        //var response = await client.SendEmailAsync(msg);
                        if (response)
                        {
                            return Ok(new ResponseBase(true, 200, "reset_mail_sent", "An email has been sent"));
                        } 
                        else
                        {
                            return BadRequest(new ResponseBase(false, 400, "reset_mail_not_sent", "an error occured"));
                        }
                    
                    } 
                    else return BadRequest(new ResponseBase(false, 400, "email_invalid" ,"This email is not related to any account"));
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
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

        /*[HttpGet]
        public IActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }*/

        /// <summary>
        /// Reset user password, (token required)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        ///     POST /users/resetPassword
        ///     
        /// </remarks>
        /// <response code="200">reset the users password</response>
        /// <response code="400">The request sent did not pass the validation, some fields must be wrong or missing</response>
        [HttpPost, Route("resetPassword")]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ResponseBase>), StatusCodes.Status400BadRequest)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var validator = new ResetPasswordValidator();
            var ValidatorResult = validator.Validate(request);
            if (ValidatorResult.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.email);
                if (user != null)
                {
                    var resetResult = await _userManager.ResetPasswordAsync(user, request.token, request.newPassword);
                    if (!resetResult.Succeeded)
                    {
                        var errors = new List<ErrorModel>();
                        foreach(var error in resetResult.Errors)
                        {
                            errors.Add(new ErrorModel
                            {
                                errorMessage = error.Description,
                                errorCode = error.Code,
                            });
                        }
                        return BadRequest(errors);
                    } 
                    else return Ok(new ResponseBase(true, 200, "reset_password_success", "Your password has been changed successfully"));
                }
                else return BadRequest(new ResponseBase(false, 400, "email_invalid", "This email is not related to any account"));
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
    }
}
