using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore22_identity_ef_sqlite.Controllers
{
    public class AppController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuthorizationService authorization;

        public AppController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IAuthorizationService authorization)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.authorization = authorization;
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
            //await ClaimAdd();
            //res += " Claim: " + await UserHasClaim();
            //res += " Policy: " + await UserCheckPolicy();

            ViewBag.result = res;
            return View();
        }


        private async Task<string> UserCreate()
        {
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

        private async Task<string> RoleCreate()
        {
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

        private async Task<bool> UserInRole()
        {
            var user = await userManager.FindByNameAsync("a");
            return await userManager.IsInRoleAsync(user, "MyRole");
        }

        private async Task ClaimAdd()
        {
            var user = await userManager.FindByNameAsync("a");
            var res = await userManager.AddClaimAsync(user, new Claim("MyClaim", "abc"));
        }

        private async Task<bool> UserHasClaim()
        {
            var user = await userManager.FindByNameAsync("a");
            var res = await userManager.GetClaimsAsync(user);
            return res.Count(i => i.Type == "MyClaim") == 1;
        }

        private async Task<bool> UserCheckPolicy()
        {            
            var res = await authorization.AuthorizeAsync(User, "IsAbc");
            return res.Succeeded;
        }
    }
}