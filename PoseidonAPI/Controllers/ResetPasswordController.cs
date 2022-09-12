using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Views.ViewModels;

namespace PoseidonAPI.Controllers
{
    public class ResetPasswordController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPasswordController> _logger;

        public ResetPasswordController(UserManager<IdentityUser> userManager, ILogger<ResetPasswordController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Index(string token)
        {
            if (token != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel()
                {
                    token = token,
                    isValidated = "",
                    emailError = "",
                    passwordError = "",
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ResetPasswordViewModel model)
        {
            _logger.LogInformation($"User : {model.Email}, route : ResetPassword/Index, callback : Index(ResetPasswordViewModel model)", DateTime.UtcNow.ToLongTimeString());

            if(model.newPassword == model.confirmPassword)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetResult = await _userManager.ResetPasswordAsync(user, model.token, model.newPassword);
                    if (resetResult.Succeeded)
                    {
                        _userManager.UpdateSecurityStampAsync(user);
                        return RedirectToAction("Success");
                    }
                    else
                    {
                        model.isValidated = "Something went wrong";
                        return View(model);
                    }
                }
                else
                {
                    model.emailError = "The Email is invalid";
                    return View(model);
                }
            } 
            else
            {
                model.passwordError = "The passwords are not the same";
                return View(model);
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
