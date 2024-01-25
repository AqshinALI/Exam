using Exam.Areas.Admin.ViewModels.Account;
using Exam.Models;
using Exam.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)return View(vm);
            AppUser user = new AppUser
            {
                Name = vm.Name,
                Email = vm.Email,
                Surname = vm.Surname,
                UserName = vm.Username
            };
            var result= await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await _userManager.FindByNameAsync(vm.UserOrEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bele istifadeci yoxdur.");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Sehv.");
                return View(vm);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Blok olundunuz.");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
            

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }

            }
            return RedirectToAction("Index", "Home");
        }
        
    }
}
