using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
          )
        {

            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<List<ApplicationUser>> GetListRecord()
        {
            var list_record = await _userManager.Users.ToListAsync();
            return list_record;
        }
        public async Task<IActionResult> Update(string? id)
        {
            string _id = id ?? "";
            var user = await _userManager.FindByIdAsync(_id);
            if (user == null)
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
            return View("CreateUpdate", user);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(IFormCollection fc, string? id)
        {
            string _id = id ?? "";
            string _name = fc["name"].ToString().Trim();
            string username = fc["userName"].ToString().Trim();
            string _password = Request.Form["password"].ToString().Trim();
            var user = await _userManager.FindByIdAsync(_id);
            if (user != null)
            {
                user.UserName = username;
                user.Name = _name;
                user.Address = "";
                user.PhoneNumber = "";
                if (!string.IsNullOrEmpty(_password))
                {
                    var passwordValidator = HttpContext.RequestServices.GetService<IPasswordValidator<ApplicationUser>>();
                    var passwordHasher = HttpContext.RequestServices.GetService<IPasswordHasher<ApplicationUser>>();
                    var result = await passwordValidator.ValidateAsync(_userManager, user, _password);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, _password);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        ViewBag.Action = "/Admin/Users/UpdatePost/" + _id;
                        return View("CreateUpdate");
                    }
                }
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
            return View("CreateUpdate");
        }
        public async Task<IActionResult> Create()
        {
            var listRoles = await _roleManager.Roles.ToListAsync();
            ViewBag.listRoles = listRoles;
            ViewBag.action = "/Admin/Users/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            
            if (ModelState.IsValid)
            {
                string name = fc["name"].ToString().Trim();
                string username = fc["userName"].ToString().Trim();
                string password = fc["password"].ToString().Trim();
                string roleName = fc["selectedRoles"].ToString().Trim();
                string email = fc["email"].ToString().Trim();
                var user = new ApplicationUser();
                user.UserName = username;
                user.Email = email;
                user.Name = name;
                user.Address = "";
                user.PhoneNumber = "";
                
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync(roleName))
                    {
                   
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                  
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.Action = "/Admin/Users/CreatePost";
            return View("CreateUpdate");
        }
        public async Task<IActionResult> Delete(string? id)
        {
            string _id = id ?? "";
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
        }
    }
}
