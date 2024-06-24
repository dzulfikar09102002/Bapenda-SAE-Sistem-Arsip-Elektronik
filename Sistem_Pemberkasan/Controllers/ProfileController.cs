using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models.EF;

namespace Sistem_Pemberkasan.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        string strViewPath = string.Empty;
        private readonly ModelContext _context;
        public List<MUser> GetUser { get; set; }
        public ProfileController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Email)
        {
            var model = new Models.Master.PegawaiVM.ProfileUser(_context, Email);
            return PartialView(strViewPath + "Index",model);
        }
    }
}
