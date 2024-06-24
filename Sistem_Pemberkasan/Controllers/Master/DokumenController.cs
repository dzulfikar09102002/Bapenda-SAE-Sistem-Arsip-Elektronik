using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models.EF;

using Sistem_Pemberkasan.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Controllers.Master
{
    [Authorize(Roles = "Pengelola")]

    public class DokumenController : Controller
    {
        private readonly ModelContext _context;
        string strViewPath = string.Empty;
        private readonly CookieData _cookieData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DokumenController(ModelContext context   , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            strViewPath = string.Concat("../Master/", GetType().Name.Replace("Controller", ""), "/");
            _cookieData = new CookieData(context, httpContextAccessor);
        }
        public IActionResult Index()
        {
            string EmailUser = _cookieData.GetUser();
            var model = new Models.Master.DokumenVM.Index(_context, EmailUser);
            return View(strViewPath + "Index", model);
        }

        public IActionResult Add()
        {
            return PartialView(strViewPath + "_Add", new Models.Master.DokumenVM.Add());
        }

        [HttpPost]
        public IActionResult dokumenBaru(Models.Master.DokumenVM.Add Model)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (string.IsNullOrEmpty(Model.NewRow.NamaDokumen))
                {
                    responseBase.Status = StatusEnum.Error;
                    responseBase.Message = "Nama harus diisi";
                    return Json(responseBase);
                }
                bool checkNamaDokumen = Models.Lib.UploadFile.CheckNamaDokumen(Model.NewRow.NamaDokumen);
                if (!checkNamaDokumen) 
                {
                    responseBase.Status = StatusEnum.Error;
                    responseBase.Message = "Dokumen sudah ada";
                    return Json(responseBase);
                }
                int id = 0;
                int? idDokumen = _context.MDokumen.DefaultIfEmpty().Max(x => (int?)x.IdDokumen);
                int statusDokumen = 1;
                string EmailUser = _cookieData.GetUser();
                var ambilNama = _context.MUsers.Where(x => x.Email == EmailUser).FirstOrDefault();

                if (idDokumen.HasValue)
                {
                    id = idDokumen.Value + 1;
                }
                else
                {
                    id = 1;
                }
                _context.MDokumen.Add(new MDokuman
                {
                    IdDokumen = id,
                    NamaDokumen = Model.NewRow.NamaDokumen,
                    DInsertBy = ambilNama.IdUser,
                    DInsertDate = DateTime.Now,
                    StatusDokumen = statusDokumen,

                });
                _context.SaveChanges();
                responseBase.Status = StatusEnum.Success;
                responseBase.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                responseBase.Status = StatusEnum.Error;
                responseBase.Message = ex.Message;
            }
            return Json(responseBase);
        }
        [Authorize(Roles = "Pengelola")]
        public IActionResult Edit(int id = -1)
        {
            Models.Master.DokumenVM.Edit model;

            TempData["TEMP_EDIT"] = id;
            model = new Models.Master.DokumenVM.Edit(_context, id);
            return PartialView(strViewPath + "_Edit", model);
        }
        
        [HttpPost]
        public ActionResult UpdateDokumen(Models.Master.DokumenVM.Edit model)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                bool checkNamaDokumen = Models.Lib.UploadFile.CheckNamaDokumen(model.DokumenRow.NamaDokumen);
                if (!checkNamaDokumen)
                {
                    responseBase.Status = StatusEnum.Error;
                    responseBase.Message = "Dokumen sudah ada";
                    return Json(responseBase);
                }
                var idKey = Convert.ToInt32(TempData["TEMP_EDIT"]);
                var check = _context.MDokumen.First(x => x.IdDokumen == idKey);
                if (check != null)
                {
                    check.NamaDokumen = model.DokumenRow.NamaDokumen;
                    check.StatusDokumen = model.DokumenRow.StatusDokumen;
                }
                _context.SaveChanges();
                responseBase.Status = StatusEnum.Success;
                responseBase.Message = "Dokumen Berhasil Diubah";
            }
            catch (Exception ex)
            {
                responseBase.Status = StatusEnum.Error;
                responseBase.Message = ex.Message;
            }


            return Json(responseBase);
        }
    }
}
