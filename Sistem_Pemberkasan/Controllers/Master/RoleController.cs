using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistem_Pemberkasan.Controllers.Master
{
    [Authorize(Roles = "Pengelola")]

    public class RoleController : Controller
    {
        string strViewPath = string.Empty;
        public RoleController()
        {
            strViewPath = string.Concat("../Master/", GetType().Name.Replace("Controller", ""), "/");
        }
        public IActionResult Index()
        {
            return PartialView(strViewPath + "Index");
        }
        public IActionResult Add()
        {
            return PartialView(strViewPath + "_Add");
        }
        public IActionResult Edit()
        {
			return PartialView(strViewPath + "_Edit");
		}
    }
}
