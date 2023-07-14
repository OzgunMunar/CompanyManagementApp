using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.LoginDTO;
using AspNetCoreMVCApp.Models;
using AspNetCoreMVCApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace AspNetCoreMVCApp.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {

        private readonly LoginRepository _loginRepository;

        public LoginController(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
                return await Task.Run(() => RedirectToAction("Index", "Home"));

            return await Task.Run(() => View());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO loginCredentials)
        {

            LoginDTO? employeeData = await _loginRepository.CheckLoginCredentialsAsync(loginCredentials);
            ViewBag.isLoginWrong = false;

            if (employeeData == null)
            {
                ViewBag.isLoginWrong = true;
                return await Task.Run(() => View("Index"));
            }
            else
            {

                List<Claim> claim = new List<Claim>
                {
                    new Claim("EmployeeName", employeeData.EmployeeName),
                    new Claim("IfAdmin", employeeData.EmployeeJobTitle)
                };

                var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(identity));

                return await Task.Run(() => RedirectToAction("Index", "Home"));

            }

        }

        public async Task<ActionResult> LogOut()
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return await Task.Run(() => RedirectToAction("Index"));

        }

    }
}
