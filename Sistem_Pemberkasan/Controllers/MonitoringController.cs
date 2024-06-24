using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models;
using Sistem_Pemberkasan.Models.EF;
using Sistem_Pemberkasan.Models.Lib;
using System;

namespace Sistem_Pemberkasan.Controllers
{
    [Authorize(Roles = "Pengelola,Perekam,User")]
    public class MonitoringController : Controller
    {
        private readonly ModelContext _context;
        string strViewPath = string.Empty;
        private readonly CookieData _cookieData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MonitoringController(ModelContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            strViewPath = string.Concat("../Transaksi/", GetType().Name.Replace("Controller", ""), "/");
            _cookieData = new CookieData(context, httpContextAccessor);
        }
        public IActionResult Index()
        {
            string EmailUser = _cookieData.GetUser();
            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.MonitoringVM.Index(_context, idUser);
            return View(strViewPath + "Index", model);
        }

        //Filter Berkas
        [HttpPost]
        public IActionResult Filter(Models.Transaksi.MonitoringVM.Index input)
        {

            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.MonitoringVM.Index(_context, idUser, input.IdKategori, input.IdBidang, input.tanggalMulai, input.tanggalAkhir);

            return PartialView(strViewPath + "_TableFilter", model);
        }
        [HttpPost]
        public IActionResult Search(Models.Transaksi.MonitoringVM.Index input)
        {
            int idUser = _cookieData.GetIdUser();

            var model = new Models.Transaksi.MonitoringVM.Index(_context, input.SearchBox);

            return PartialView(strViewPath + "_TableFilter", model);
        }
        public IActionResult DetailDokumen(string IdBerkas, string IdKategori)
        {
            int idUser = _cookieData.GetIdUser();
            var model = new Models.Transaksi.MonitoringVM.Detail(_context, Convert.ToInt32(IdBerkas), Convert.ToInt32(IdKategori) ,idUser);
            return PartialView(strViewPath + "_DetailDokumen", model);
        }
        public IActionResult AddPendukung(string IdBerkas)
        {
            var model = new Models.Transaksi.MonitoringVM.addPendukung(_context, Convert.ToInt32(IdBerkas));
            return PartialView(strViewPath + "_AddPendukung", model);
        }

        [HttpPost]
        public async Task<IActionResult> PendukungBaru(Models.Transaksi.MonitoringVM.addPendukung model)
        {
            ResponseBase responseBase = new ResponseBase();

            try
            {
                if (string.IsNullOrEmpty(model.NewRowPendukung.KeteranganPendukungBerkas))
                {
                    responseBase.Status = StatusEnum.Error;
                    responseBase.Message = "Keterangan tidak boleh kosong";
                    return Json(responseBase);
                }
                int id = 0;
                int statusPendukung = 1;
                string EmailUser = _cookieData.GetUser();
                var ambilNama = _context.MUsers.Where(x => x.Email == EmailUser).FirstOrDefault();
                int? idPendukung = _context.PendukungBerkas.DefaultIfEmpty().Max(x => (int?)x.IdPendukungBerkas);
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (idPendukung.HasValue)
                {
                    id = idPendukung.Value + 1;
                }
                else
                {
                    id = 1;
                }
                if (model.file != null)
                {
                    var fileName = model.berkasId.IdBerkas + "-" + id + "-" + dateTime + ".pdf";
                    byte[] fileBloob;
                    using (var ms = new MemoryStream())
                    {
                        // Salin file ke MemoryStream
                        model.file.CopyTo(ms);

                        // Konversi stream ke array byte
                        fileBloob = ms.ToArray();
                    }
                    _context.PendukungBerkas.Add(new PendukungBerka
                    {
                        IdPendukungBerkas = id,
                        IdKategoriPendukungBerkas = model.NewPendukung.IdKategoriPendukungBerkas,
                        IdBerkas = model.berkasId.IdBerkas,
                        KeteranganPendukungBerkas = model.NewRowPendukung.KeteranganPendukungBerkas,
                        StatusPendukungBerkas = statusPendukung,
                        NamaDokumenPendukungBerkas = fileName,
                        FileBloob = fileBloob,
                        PbInsertBy = ambilNama.IdUser,
                        PbInsertDate = DateTime.Now,
                    });
                    _context.SaveChanges();
                    responseBase.Message = "File Pendukung tersimpan";
                    responseBase.Status = StatusEnum.Success;

                }
                else
                {
                    responseBase.Message = "File Tidak Boleh Kosong";
                    responseBase.Status = StatusEnum.Error;
                }
               
                
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                responseBase.Status = StatusEnum.Error;
            }



            return Json(responseBase);
        }

        public IActionResult EditPendukung(string IdBerkas)
        {
            var model = new Models.Transaksi.MonitoringVM.editPendukung(_context, Convert.ToInt32(IdBerkas));
            return PartialView(strViewPath + "_EditPendukung", model);
        }

        [HttpPost]
        public async Task<IActionResult> saveEditPendukung(Models.Transaksi.MonitoringVM.SaveEditPendukung model, string idPendukung)
        {
            ResponseBase responseBase = new ResponseBase();
            string EmailUser = _cookieData.GetUser();
            var ambilNama = _context.MUsers.FirstOrDefault(x => x.Email == EmailUser);
            var IdPendukung = Convert.ToInt32(idPendukung);

            var CheckFiles = _context.PendukungBerkas.FirstOrDefault(x => x.IdPendukungBerkas == IdPendukung);

            try
            {

                if (model.file != null)
                {
                    var fileName = CheckFiles.NamaDokumenPendukungBerkas;

                    byte[] fileBloob;
                    using (var ms = new MemoryStream())
                    {
                        // Salin file ke MemoryStream
                        model.file.CopyTo(ms);

                        // Konversi stream ke array byte
                        fileBloob = ms.ToArray();
                    }
                    CheckFiles.NamaDokumenPendukungBerkas = fileName;
                    CheckFiles.FileBloob = fileBloob;
                }
               
                if (model.NewRow.KeteranganPendukungBerkas != null ) 
                {
                CheckFiles.KeteranganPendukungBerkas = model.NewRow.KeteranganPendukungBerkas;
                }
                CheckFiles.StatusPendukungBerkas = model.NewRow.StatusPendukungBerkas;
                CheckFiles.PbInsertBy = ambilNama.IdUser;
                CheckFiles.PbInsertDate = DateTime.Now;
                _context.SaveChanges();

                responseBase.Message = "File Pendukung tersimpan";
                responseBase.Status = StatusEnum.Success;
            }
            catch (DbUpdateException ex)
            {
                responseBase.Message = "Gagal menyimpan perubahan ke database. Error: " + ex.Message;
                responseBase.Status = StatusEnum.Error;
            }
            catch (Exception ex)
            {
                responseBase.Message = "Terjadi kesalahan. Error: " + ex.Message;
                responseBase.Status = StatusEnum.Error;
            }

            return Json(responseBase);
        }
        [HttpGet]
        public IActionResult DownloadCompressedFile(string namaDetailDokumen)
        {
            var fileInfo = _context.DetailDokumen.FirstOrDefault(f => f.NamaDetailDokumen == namaDetailDokumen);

            var mimeType = Models.Lib.UploadFile.GetJenisFile(fileInfo.FileBloob);
            if (fileInfo == null)
            {
                return NotFound();
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileInfo.NamaDetailDokumen,
                Inline = true,
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(fileInfo.FileBloob, mimeType.ContentType);
        }
        [HttpGet]
        public IActionResult DownloadPendukung(string namaDetailDokumen)
        {
            var fileInfo = _context.PendukungBerkas.FirstOrDefault(f => f.NamaDokumenPendukungBerkas == namaDetailDokumen);

            var mimeType = Models.Lib.UploadFile.GetJenisFile(fileInfo.FileBloob);
            if (fileInfo == null)
            {
                return NotFound();
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileInfo.NamaDokumenPendukungBerkas,
                Inline = true,
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(fileInfo.FileBloob, mimeType.ContentType);
        }

    }
}
