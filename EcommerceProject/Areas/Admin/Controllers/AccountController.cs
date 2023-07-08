using EcommerceProject.DTO;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
          )
        {

            this.userManager = userManager;
            this._signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Login()
        {

            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> LoginPostAdmin(IFormCollection fc)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            var model = new LoginModel();
            model.Username = fc["Username"].ToString().Trim();
            model.Email = fc["Email"].ToString().Trim();
            model.Password = fc["Password"].ToString().Trim();
            var checkbox = fc["RememberMe"].ToString();
            var RemeberMeBool = (checkbox == "on") ? true : false;
            model.RememberMe = RemeberMeBool;
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Redirect("/Admin/Home/Index");

                }
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Registration()
        {
            return View("Registration");
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> RegistrationPost(IFormCollection fc)
        {
            string errorMessage = "";
            if (!ModelState.IsValid)
            {
                return View("Registration");
            }
            var regisModel = new RegistraionModel();
            regisModel.Username = fc["username"].ToString().Trim();
            regisModel.Email = fc["email"].ToString().Trim();
            regisModel.Name = fc["name"].ToString().Trim();
            regisModel.Address = fc["address"].ToString().Trim();
            regisModel.NumberPhone = fc["numberphone"].ToString().Trim();
            var existingUser = await userManager.FindByEmailAsync(regisModel.Email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessages = "Người dùng đã tồn tại";
                return View("Registration");
            }

            var userRegis = new ApplicationUser();
            userRegis.UserName = regisModel.Username;
            userRegis.Email = regisModel.Email;
            userRegis.Name = regisModel.Name;
            userRegis.Address = regisModel.Address;
            userRegis.PhoneNumber = regisModel.NumberPhone;
            var result = await userManager.CreateAsync(userRegis, fc["password"].ToString().Trim());
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userRegis, isPersistent: false);

                await userManager.AddToRoleAsync(userRegis, "Admin");

                return Redirect("/Admin/Home/Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {

                    errorMessage = error.Description;

                }
                ViewBag.ErrorMessages = errorMessage;
            }
            return View("Registration");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
