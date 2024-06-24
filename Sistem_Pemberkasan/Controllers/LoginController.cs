using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sistem_Pemberkasan.Models.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Sistem_Pemberkasan.Models.Lib;


namespace PenagihanPADWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ModelContext _context;
        string strViewPath = string.Empty;

        public LoginController(ILogger<LoginController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string? returnUrl = null)
        {
            var claims = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (claims != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            var model = new Models.Login.LoginVM();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.Login.LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkUser = _context.MUsers.Include(x => x.IdRoleNavigation).Include(x => x.NikNavigation).ThenInclude(x => x.IdBidangNavigation).Where(x => x.StatusUser == 1).FirstOrDefault(x => x.Email == model.Email);

                    if (checkUser == null)
                    {
                        throw new Exception("Akun Tidak Ditemukan / Akun Sudah Tidak Aktif");
                    }
                    else
                    {
                        string namaBidang = checkUser.NikNavigation.IdBidangNavigation.NamaBidang;
                    }
                    var hasher = new PasswordHasher<MUser>();
                    var result = hasher.VerifyHashedPassword(checkUser, checkUser.Password, model.Password);
                    if (result != PasswordVerificationResult.Success)
                    {
                        throw new Exception("Email Atau Kata Sandi Salah!");
                    }
                    var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, checkUser.Email),
                    new Claim(ClaimTypes.Role, checkUser.IdRoleNavigation != null ?  checkUser.IdRoleNavigation.JenisRole :""),
                    new Claim(ClaimTypes.Actor, checkUser.NikNavigation != null ?   checkUser.NikNavigation.IdBidangNavigation.NamaBidang:""),
                   
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return LocalRedirect(model.ReturnUrl ?? Url.Content("~/Home"));
                }
                else
                {
                    throw new Exception("Form Harus Diisi");
                }
            }
            catch (Exception ex)
            {
                TempData["error_login"] = ex.Message;
                return View("Index", model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Clear();
            return LocalRedirect("/Login"); // Redirect to login page after logout
        }

    }
}
