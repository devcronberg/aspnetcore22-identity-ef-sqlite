using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore22_identity_ef_sqlite.Controllers
{
    public class AppController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AppController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet("~/")]
        public async Task<IActionResult> Index()
        {
            string res = "";
            //string res = await UserCreate();
            //res += await UserSignIn();
            //res += " " + UserSignedIn();
            //res += " " + await UserSignOut();
            //res = await RoleCreate();
            //res = await UserAddToRole();
            ViewBag.result = res;            
            return View();
        }


        private async Task<string> UserCreate() {
            var user = new IdentityUser { UserName = "a", Email = "a@a.dk" };
            var result = await userManager.CreateAsync(user, "a");
            return "ok - user created";
        }

        private async Task<string> UserSignIn()
        {
            var user = await userManager.FindByNameAsync("a");
            var result = await signInManager.PasswordSignInAsync(user, "a", true, false);
            if (result.Succeeded)
                return "ok - user signed in";
            else
                return "Error signing in";
        }

        private string UserSignedIn()
        {            
            return "SignedIn: " + signInManager.IsSignedIn(User).ToString();
        }

        private async Task<string> UserSignOut()
        {
            await signInManager.SignOutAsync();
            return "After sign out... Signed in" + signInManager.IsSignedIn(User).ToString();
        }

        private async Task<string> RoleCreate() {
            var res = await roleManager.CreateAsync(new IdentityRole { Name = "MyRole" });
            if (res.Succeeded)
                return "ok - role created";
            else
                return "nok - role not created";
        }
        private async Task<string> UserAddToRole()
        {
            var user = await userManager.FindByNameAsync("a");
            var res = await userManager.AddToRoleAsync(user, "MyRole");
            if (res.Succeeded)
                return "ok - role created added to user";
            else
                return "nok - role not added created to user";
        }

        // Missing
        //var res = await userManager.AddClaimAsync(signedUser, new Claim("MyClaim", "abc"));

    }
}