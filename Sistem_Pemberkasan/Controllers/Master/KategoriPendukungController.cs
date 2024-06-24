using Microsoft.AspNetCore.Mvc;
using Sistem_Pemberkasan.Models.EF;

using Sistem_Pemberkasan.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Sistem_Pemberkasan.Models.Lib;

namespace Sistem_Pemberkasan.Controllers.Master
{
    [Authorize(Roles = "Pengelola")]
    public class KategoriPendukungController : Controller
    {
        private readonly ModelContext _context;
        string strViewPath = string.Empty;
        private readonly CookieData _cookieData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public KategoriPendukungController(ModelContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            strViewPath = string.Concat("../Master/", GetType().Name.Replace("Controller", ""), "/");
            _cookieData = new CookieData(context, httpContextAccessor);
        }
        public IActionResult Index()
        {
            string EmailUser = _cookieData.GetUser();
            var model = new Models.Master.PendukungVM.Index(_context);
            return View(strViewPath + "Index", model);
        }

        public IActionResult Add()
        {
            return PartialView(strViewPath + "_Add", new Models.Master.PendukungVM.Add());
        }

        [HttpPost]
        public IActionResult pendukungBaru(Models.Master.PendukungVM.Add Model)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (string.IsNullOrEmpty(Model.NewRowBerka.NamaKategoriPendukungBerkas))
                {
                    responseBase.Status = StatusEnum.Error;
                    responseBase.Message = "Nama harus diisi";
                    return Json(responseBase);
                }
                int id = 0;
                int? idPendukung = _context.MKategoriPendukungBerkas.DefaultIfEmpty().Max(x => (int?)x.IdKategoriPendukungBerkas);
                int statusPendukung = 1;
                string EmailUser = _cookieData.GetUser();
                var ambilNama = _context.MUsers.Where(x => x.Email == EmailUser).FirstOrDefault();

                if (idPendukung.HasValue)
                {
                    id = idPendukung.Value + 1;
                }
                else
                {
                    id = 1;
                }
                _context.MKategoriPendukungBerkas.Add(new MKategoriPendukungBerka
                {
                    IdKategoriPendukungBerkas = id,
                    NamaKategoriPendukungBerkas = Model.NewRowBerka.NamaKategoriPendukungBerkas,
                    KpInsertBy = ambilNama.IdUser,
                    KpInsertDate = DateTime.Now,
                    StatusKategoriPendukung = statusPendukung,
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
            Models.Master.PendukungVM.Edit model;
            TempData["TEMP_EDIT"] = id;
            model = new Models.Master.PendukungVM.Edit(_context, id);
            return PartialView(strViewPath + "_Edit", model);
        }

        [HttpPost]
        public ActionResult updatePendukung(Models.Master.PendukungVM.Edit model)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var idKey = Convert.ToInt32(TempData["TEMP_EDIT"]);
                var check = _context.MKategoriPendukungBerkas.First(x => x.IdKategoriPendukungBerkas == idKey);
                if (check != null)
                {
                    check.NamaKategoriPendukungBerkas = model.KategoriPendukungRow.NamaKategoriPendukungBerkas;
                    check.StatusKategoriPendukung = model.KategoriPendukungRow.StatusKategoriPendukung;
                }
                _context.SaveChanges();
                responseBase.Status = StatusEnum.Success;
                responseBase.Message = "Data Berhasil Diubah";
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
