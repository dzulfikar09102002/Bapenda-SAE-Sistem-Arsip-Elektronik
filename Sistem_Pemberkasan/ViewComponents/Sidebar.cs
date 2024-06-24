using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;
using System.Security.Claims;

namespace Sistem_Pemberkasan.ViewComponents
{
    public class Sidebar : ViewComponent
    {
        private readonly ModelContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Sidebar(ModelContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string name = "";
            string role = "";
           
            if (_httpContextAccessor.HttpContext != null)
            {
                var getClaimName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                var getClaimRole = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

                if (getClaimName == null)
                {
                    throw new Exception("Claim Null Name ");
                }
                if (getClaimRole == null)
                {
                    throw new Exception("Claim Null USer");
                }
                name = getClaimName.Value;
                role = getClaimRole.Value;
            }
            else
            {
                throw new Exception("Context Null");
            }

            var model = new SidebarVC();
            try
            {

                //var getRole = await _context.MRoles
                //    .Include(x => x.MRouteRoles)
                //    .ThenInclude(x => x.IdRouteNavigation)
                //    .Where(x => x.JenisRole == role)
                //    .FirstOrDefaultAsync();

                var getRole = await _context.MRoles
                   .Where(x => x.JenisRole == role)
                   .FirstOrDefaultAsync();

                var getNemeUser = await _context.MUsers
                    .Where(x => x.Email == name)
                    .FirstOrDefaultAsync();

                if (getNemeUser == null)
                {
                    throw new Exception("User Tidak Terdaftar");
                }
                if (getRole == null)
                {
                    throw new Exception("User Tidak Sah / Kosong");
                }
                model.Role = getRole;

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(model);
            }
        }
    }
    public class SidebarVC
    {
        public MRole Role { get; set; } = new MRole();
    }
}
